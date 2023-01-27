using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Runtime.Serialization;
using UnityEngine;
using System.Threading;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;

public class StubComponent : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Initializing()
    {
        ShootsStub.GetInstance();
    }

    public void Awake()
    {
        Application.runInBackground = true;
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        ShootsStub.GetInstance().HandleActions();
    }

    public void OnDestroy()
    {
        ShootsStub.GetInstance().CleanUp();
    }

    public void OnGUI()
    {
        ShootsStub.GetInstance().GetStubServer().ShowHighlight();
    }
}


public class ShootsStub
{
    private static ShootsStub instance;
    private static GameObject stubGameObject;
    private static readonly object locker = new object();
    private readonly StubServer server;

    public ShootsStub()
    {
        server = new StubServer();
    }

    public static ShootsStub GetInstance()
    {
        if (instance == null)
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new ShootsStub();
                    stubGameObject = new GameObject()
                    {
                        name = "__ShootsStub__"
                    };
                    stubGameObject.AddComponent<StubComponent>();
                    instance.GetStubServer().SetStubNode(stubGameObject);
                }
            }
        }
        return instance;
    }

    public void HandleActions()
    {
        server.HandleActions();
    }

    public void CleanUp()
    {
        server.CleanUp();
    }

    public StubServer GetStubServer()
    {
        return server;
    }
}

[Serializable]
public class StubRequest
{
    public int id;
    public string jsonrpc;
    public string method;
    public Dictionary<string, object> @params;
}

[Serializable]
public class StubResponse
{
    public int id;
    public string jsonrpc;
    public object result;
}

[Serializable]
public class StubErrorResponse
{
    public int id;
    public string jsonrpc;
    public Dictionary<string, object> error;
}

public static class JSONParser
{
    [ThreadStatic] static Stack<List<string>> splitArrayPool;
    [ThreadStatic] static StringBuilder stringBuilder;
    [ThreadStatic] static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache;
    [ThreadStatic] static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache;

    public static T FromJson<T>(string json)
    {
        // Initialize, if needed, the ThreadStatic variables
        if (propertyInfoCache == null) propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        if (fieldInfoCache == null) fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
        if (stringBuilder == null) stringBuilder = new StringBuilder();
        if (splitArrayPool == null) splitArrayPool = new Stack<List<string>>();

        //Remove all whitespace not within strings to make parsing simpler
        stringBuilder.Length = 0;
        for (int i = 0; i < json.Length; i++)
        {
            char c = json[i];
            if (c == '"')
            {
                i = AppendUntilStringEnd(true, i, json);
                continue;
            }
            if (char.IsWhiteSpace(c))
                continue;

            stringBuilder.Append(c);
        }

        //Parse the thing!
        return (T)ParseValue(typeof(T), stringBuilder.ToString());
    }

    static int AppendUntilStringEnd(bool appendEscapeCharacter, int startIdx, string json)
    {
        stringBuilder.Append(json[startIdx]);
        for (int i = startIdx + 1; i < json.Length; i++)
        {
            if (json[i] == '\\')
            {
                if (appendEscapeCharacter)
                    stringBuilder.Append(json[i]);
                stringBuilder.Append(json[i + 1]);
                i++;//Skip next character as it is escaped
            }
            else if (json[i] == '"')
            {
                stringBuilder.Append(json[i]);
                return i;
            }
            else
                stringBuilder.Append(json[i]);
        }
        return json.Length - 1;
    }

    //Splits { <value>:<value>, <value>:<value> } and [ <value>, <value> ] into a list of <value> strings
    static List<string> Split(string json)
    {
        List<string> splitArray = splitArrayPool.Count > 0 ? splitArrayPool.Pop() : new List<string>();
        splitArray.Clear();
        if (json.Length == 2)
            return splitArray;
        int parseDepth = 0;
        stringBuilder.Length = 0;
        for (int i = 1; i < json.Length - 1; i++)
        {
            switch (json[i])
            {
                case '[':
                case '{':
                    parseDepth++;
                    break;
                case ']':
                case '}':
                    parseDepth--;
                    break;
                case '"':
                    i = AppendUntilStringEnd(true, i, json);
                    continue;
                case ',':
                case ':':
                    if (parseDepth == 0)
                    {
                        splitArray.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        continue;
                    }
                    break;
            }

            stringBuilder.Append(json[i]);
        }

        splitArray.Add(stringBuilder.ToString());

        return splitArray;
    }

    internal static object ParseValue(Type type, string json)
    {
        if (type == typeof(string))
        {
            if (json.Length <= 2)
                return string.Empty;
            StringBuilder parseStringBuilder = new StringBuilder(json.Length);
            for (int i = 1; i < json.Length - 1; ++i)
            {
                if (json[i] == '\\' && i + 1 < json.Length - 1)
                {
                    int j = "\"\\nrtbf/".IndexOf(json[i + 1]);
                    if (j >= 0)
                    {
                        parseStringBuilder.Append("\"\\\n\r\t\b\f/"[j]);
                        ++i;
                        continue;
                    }
                    if (json[i + 1] == 'u' && i + 5 < json.Length - 1)
                    {
                        uint c = uint.Parse(json.Substring(i + 2, 4),
                            System.Globalization.NumberStyles.AllowHexSpecifier, null);
                        parseStringBuilder.Append((char)c);
                        i += 5;
                        continue;
                    }
                }
                parseStringBuilder.Append(json[i]);
            }
            return Regex.Unescape(parseStringBuilder.ToString());
        }
        if (type.IsPrimitive)
        {
            var result = Convert.ChangeType(json, type, System.Globalization.CultureInfo.InvariantCulture);
            return result;
        }
        if (type == typeof(decimal))
        {
            decimal result = decimal.Parse(json,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture);
            return result;
        }
        if (json == "null")
        {
            return null;
        }
        if (type.IsEnum)
        {
            if (json[0] == '"')
                json = json.Substring(1, json.Length - 2);
            try
            {
                return Enum.Parse(type, json, false);
            }
            catch
            {
                return 0;
            }
        }
        if (type.IsArray)
        {
            Type arrayType = type.GetElementType();
            if (json[0] != '[' || json[json.Length - 1] != ']')
                return null;

            List<string> elems = Split(json);
            Array newArray = Array.CreateInstance(arrayType, elems.Count);
            for (int i = 0; i < elems.Count; i++)
                newArray.SetValue(ParseValue(arrayType, elems[i]), i);
            splitArrayPool.Push(elems);
            return newArray;
        }
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            Type listType = type.GetGenericArguments()[0];
            if (json[0] != '[' || json[json.Length - 1] != ']')
                return null;

            List<string> elems = Split(json);
            var list = (IList)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count });
            for (int i = 0; i < elems.Count; i++)
                list.Add(ParseValue(listType, elems[i]));
            splitArrayPool.Push(elems);
            return list;
        }
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            Type keyType, valueType;
            {
                Type[] args = type.GetGenericArguments();
                keyType = args[0];
                valueType = args[1];
            }

            //Refuse to parse dictionary keys that aren't of type string
            if (keyType != typeof(string))
                return null;
            //Must be a valid dictionary element
            if (json[0] != '{' || json[json.Length - 1] != '}')
                return null;
            //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
            List<string> elems = Split(json);
            if (elems.Count % 2 != 0)
                return null;

            var dictionary = (IDictionary)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count / 2 });
            for (int i = 0; i < elems.Count; i += 2)
            {
                if (elems[i].Length <= 2)
                    continue;
                string keyValue = elems[i].Substring(1, elems[i].Length - 2);
                object val = ParseValue(valueType, elems[i + 1]);
                dictionary.Add(keyValue, val);
            }
            return dictionary;
        }
        if (type == typeof(object))
        {
            return ParseAnonymousValue(json);
        }
        if (json[0] == '{' && json[json.Length - 1] == '}')
        {
            return ParseObject(type, json);
        }

        return null;
    }

    static object ParseAnonymousValue(string json)
    {
        if (json.Length == 0)
            return null;
        if (json[0] == '{' && json[json.Length - 1] == '}')
        {
            List<string> elems = Split(json);
            if (elems.Count % 2 != 0)
                return null;
            var dict = new Dictionary<string, object>(elems.Count / 2);
            for (int i = 0; i < elems.Count; i += 2)
                dict.Add(elems[i].Substring(1, elems[i].Length - 2), ParseAnonymousValue(elems[i + 1]));
            return dict;
        }
        if (json[0] == '[' && json[json.Length - 1] == ']')
        {
            List<string> items = Split(json);
            var finalList = new List<object>(items.Count);
            for (int i = 0; i < items.Count; i++)
                finalList.Add(ParseAnonymousValue(items[i]));
            return finalList;
        }
        if (json[0] == '"' && json[json.Length - 1] == '"')
        {
            string str = Regex.Unescape(json.Substring(1, json.Length - 2));
            return str.Replace("\\", string.Empty);
        }
        if (char.IsDigit(json[0]) || json[0] == '-')
        {
            if (json.Contains("."))
            {
                double result = double.Parse(json,
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture);
                return result;
            }
            else
            {
                return ParseInteger(json, json[0] == '-');
            }
        }
        if (json == "true")
            return true;
        if (json == "false")
            return false;
        return null;
    }

    static object ParseInteger(string value, bool signed)
    {
        int result32;
        if (int.TryParse(value, out result32))
        {
            return result32;
        }
        Int64 result64;
        if (Int64.TryParse(value, out result64))
        {
            return result64;
        }
        UInt64 resultU64;
        if (UInt64.TryParse(value, out resultU64))
        {
            return resultU64;
        }
        throw new Exception(string.Format("invalid integer string: {0}", value));
    }

    static Dictionary<string, T> CreateMemberNameDictionary<T>(T[] members) where T : MemberInfo
    {
        Dictionary<string, T> nameToMember = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < members.Length; i++)
        {
            T member = members[i];
            if (member.IsDefined(typeof(IgnoreDataMemberAttribute), true))
                continue;

            string name = member.Name;
            if (member.IsDefined(typeof(DataMemberAttribute), true))
            {
                DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
                if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                    name = dataMemberAttribute.Name;
            }

            nameToMember.Add(name, member);
        }

        return nameToMember;
    }

    static object ParseObject(Type type, string json)
    {
        object instance = FormatterServices.GetUninitializedObject(type);

        //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
        List<string> elems = Split(json);
        if (elems.Count % 2 != 0)
            return instance;

        Dictionary<string, FieldInfo> nameToField;
        Dictionary<string, PropertyInfo> nameToProperty;
        if (!fieldInfoCache.TryGetValue(type, out nameToField))
        {
            nameToField = CreateMemberNameDictionary(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
            fieldInfoCache.Add(type, nameToField);
        }
        if (!propertyInfoCache.TryGetValue(type, out nameToProperty))
        {
            nameToProperty = CreateMemberNameDictionary(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
            propertyInfoCache.Add(type, nameToProperty);
        }

        for (int i = 0; i < elems.Count; i += 2)
        {
            if (elems[i].Length <= 2)
                continue;
            string key = elems[i].Substring(1, elems[i].Length - 2);
            string value = elems[i + 1];

            FieldInfo fieldInfo;
            PropertyInfo propertyInfo;
            if (nameToField.TryGetValue(key, out fieldInfo))
                fieldInfo.SetValue(instance, ParseValue(fieldInfo.FieldType, value));
            else if (nameToProperty.TryGetValue(key, out propertyInfo))
                propertyInfo.SetValue(instance, ParseValue(propertyInfo.PropertyType, value), null);
        }
        return instance;
    }
}

public static class JSONWriter
{
    public static string ToJson(object item)
    {
        StringBuilder stringBuilder = new StringBuilder();
        AppendValue(stringBuilder, item);
        return stringBuilder.ToString();
    }

    static void AppendValue(StringBuilder stringBuilder, object item)
    {
        if (item == null)
        {
            stringBuilder.Append("null");
            return;
        }

        Type type = item.GetType();
        if (type == typeof(string) || type == typeof(char))
        {
            stringBuilder.Append('"');
            string str = item.ToString();
            for (int i = 0; i < str.Length; ++i)
                if (str[i] < ' ' || str[i] == '"' || str[i] == '\\')
                {
                    stringBuilder.Append('\\');
                    int j = "\"\\\n\r\t\b\f".IndexOf(str[i]);
                    if (j >= 0)
                        stringBuilder.Append("\"\\nrtbf"[j]);
                    else
                        stringBuilder.AppendFormat("u{0:X4}", (UInt32)str[i]);
                }
                else
                    stringBuilder.Append(str[i]);
            stringBuilder.Append('"');
        }
        else if (type == typeof(byte) || type == typeof(sbyte))
        {
            stringBuilder.Append(item.ToString());
        }
        else if (type == typeof(short) || type == typeof(ushort))
        {
            stringBuilder.Append(item.ToString());
        }
        else if (type == typeof(int) || type == typeof(uint))
        {
            stringBuilder.Append(item.ToString());
        }
        else if (type == typeof(long) || type == typeof(ulong))
        {
            stringBuilder.Append(item.ToString());
        }
        else if (type == typeof(float))
        {
            stringBuilder.Append(((float)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        else if (type == typeof(double))
        {
            stringBuilder.Append(((double)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        else if (type == typeof(decimal))
        {
            stringBuilder.Append(((decimal)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        else if (type == typeof(bool))
        {
            stringBuilder.Append(((bool)item) ? "true" : "false");
        }
        else if (type.IsEnum)
        {
            stringBuilder.Append('"');
            stringBuilder.Append(item.ToString());
            stringBuilder.Append('"');
        }
        else if (item is IList)
        {
            stringBuilder.Append('[');
            bool isFirst = true;
            IList list = item as IList;
            for (int i = 0; i < list.Count; i++)
            {
                if (isFirst)
                    isFirst = false;
                else
                    stringBuilder.Append(',');
                AppendValue(stringBuilder, list[i]);
            }
            stringBuilder.Append(']');
        }
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            Type keyType = type.GetGenericArguments()[0];

            //Refuse to output dictionary keys that aren't of type string
            if (keyType != typeof(string))
            {
                stringBuilder.Append("{}");
                return;
            }

            stringBuilder.Append('{');
            IDictionary dict = item as IDictionary;
            bool isFirst = true;
            foreach (object key in dict.Keys)
            {
                if (isFirst)
                    isFirst = false;
                else
                    stringBuilder.Append(',');
                stringBuilder.Append('\"');
                stringBuilder.Append((string)key);
                stringBuilder.Append("\":");
                AppendValue(stringBuilder, dict[key]);
            }
            stringBuilder.Append('}');
        }
        else
        {
            stringBuilder.Append('{');

            bool isFirst = true;
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                if (fieldInfos[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    continue;

                object value = fieldInfos[i].GetValue(item);
                if (isFirst)
                    isFirst = false;
                else
                    stringBuilder.Append(',');
                stringBuilder.Append('\"');
                stringBuilder.Append(GetMemberName(fieldInfos[i]));
                stringBuilder.Append("\":");
                AppendValue(stringBuilder, value);

            }
            PropertyInfo[] propertyInfo = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (!propertyInfo[i].CanRead || propertyInfo[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    continue;

                object value = propertyInfo[i].GetValue(item, null);
                if (isFirst)
                    isFirst = false;
                else
                    stringBuilder.Append(',');
                stringBuilder.Append('\"');
                stringBuilder.Append(GetMemberName(propertyInfo[i]));
                stringBuilder.Append("\":");
                AppendValue(stringBuilder, value);

            }

            stringBuilder.Append('}');
        }
    }

    static string GetMemberName(MemberInfo member)
    {
        if (member.IsDefined(typeof(DataMemberAttribute), true))
        {
            DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
            if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                return dataMemberAttribute.Name;
        }
        return member.Name;
    }
}

public class StubClient
{
    private Socket s;
    private readonly StubServer svr;
    private readonly byte[] header = new byte[4];
    private byte[] body;

    public StubClient(Socket client, StubServer server)
    {
        s = client;
        svr = server;
        StartReceiving();
    }

    private void StartReceiving()
    {
        s.BeginReceive(header, 0, header.Length, SocketFlags.None, new AsyncCallback(OnReceiveHeader), s);
    }

    private void OnReceiveHeader(IAsyncResult iar)
    {
        s = (Socket)iar.AsyncState;
        int read = s.EndReceive(iar);
        if (read == 0)
        {
            UnityEngine.Debug.Log("[SHOOTS_UNITY]no header bytes received, close socket");
            s.Close();
            svr.RemoveClient(this);
            return;
        }

        int length = BitConverter.ToInt32(header, 0);
        body = new byte[length - 4];
        s.BeginReceive(body, 0, length - 4, SocketFlags.None, new AsyncCallback(OnReceiveBody), s);
    }

    private void OnReceiveBody(IAsyncResult iar)
    {
        s = (Socket)iar.AsyncState;
        int read = s.EndReceive(iar);
        if (read == 0)
        {
            UnityEngine.Debug.Log("[SHOOTS_UNITY]no body bytes received, close socket");
            s.Close();
            svr.RemoveClient(this);
            return;
        }
        StubRequest req = null;
        try
        {
            req = JSONParser.FromJson<StubRequest>(Encoding.UTF8.GetString(body));
            object rsp = svr.HandleRequest(req);
            SendResponse(rsp);
        }
        catch (Exception e)
        {
            StubErrorResponse rsp = new StubErrorResponse
            {
                id = req == null ? -1 : req.id,
                jsonrpc = "2.0",
                error = new Dictionary<string, object>()
            };
            rsp.error["code"] = -32000;
            rsp.error["message"] = string.Format("invalid request: {0}\n{1}", e.Message, e.StackTrace);
            SendResponse(rsp);
        }
    }

    private void SendResponse(object rsp)
    {
        string jsonString = JSONWriter.ToJson(rsp);
        byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonString);
        int length = bodyBytes.Length + 4;
        byte[] buffer = new byte[length];
        Buffer.BlockCopy(BitConverter.GetBytes(length), 0, buffer, 0, 4);
        Buffer.BlockCopy(bodyBytes, 0, buffer, 4, bodyBytes.Length);
        s.BeginSend(buffer, 0, length, SocketFlags.None, new AsyncCallback(OnSend), s);
    }

    private void OnSend(IAsyncResult iar)
    {
        s = (Socket)iar.AsyncState;
        s.EndSend(iar);
        StartReceiving();
    }
}

public class ActionContext
{
    public Dictionary<string, object> data;
    public EventWaitHandle handle;
    public object result;
    public bool raised = false;
}

public class ActionError : Exception
{
    public int Code;
    public new string Message;

    public ActionError(int code, string message)
    {
        Code = code;
        Message = message;
    }
}

public class PredicateItem
{
    public string key;
    public string op;
    public object value;
}

public class UPathItem
{
    public List<object> predicates;
    public object index;
    public object depth;

    private delegate bool opFunc(object value);
    private Dictionary<string, Regex> regs = new Dictionary<string, Regex>();


    public bool IsMatch(GameObject obj)
    {
        foreach (Dictionary<string, object> predicate in predicates)
        {
            string key = (string)predicate["name"];
            string op = (string)predicate["operator"];
            object value = predicate["value"];
            object target = GetObjectValue(obj, key);
            bool bResult = false;
            if (target == null)
            {
                return false;
            }
            if (op == "==")
            {
                bResult = target.Equals(value);
            }
            else if (op == "~=")
            {
                bResult = RegexMatch(value, target);
            }
            else if (op == "!=")
            {
                bResult = target != value;
            }
            if (!bResult)
            {
                return false;
            }
        }
        return true;
    }

    public bool RegexMatch(object regex, object target)
    {
        if (!(regex is string))
        {
            string message = string.Format("regex with type={0} value={1} is illegal", regex.GetType(), regex);
            throw new ActionError(-32000, message);
        }
        if (!(target is string))
        {
            string message = string.Format("target with type={0} value={1} is illegal", target.GetType(), target);
            throw new ActionError(-32000, message);
        }
        string stringRegex = (string)regex;
        string stringTarget = (string)target;
        if (!regs.ContainsKey(stringRegex))
        {
            regs[stringRegex] = new Regex(stringRegex, RegexOptions.IgnoreCase);
        }
        Regex reg = regs[stringRegex];
        return reg.IsMatch(stringTarget);
    }

    private object GetObjectValue(GameObject obj, string key)
    {
        if (key == "tag")
        {
            return obj.tag;
        }
        else if (key == "name")
        {
            return obj.name;
        }
        else if (key == "text")
        {
            return StubServer.GetGameObjectText(obj);
        }
        else if (key == "visible")
        {
            return StubServer.IsGameObjectVisible(obj);
        }
        else if (key == "type")
        {
            throw new ActionError(-32000, "type is not supported yet");
        }
        else if (key == "texture")
        {
            return StubServer.GetGameObjectTexture(obj);
        }
        else if (key == "enabled")
        {
            return StubServer.IsGameObjectEnabled(obj);
        }
        else if (key == "checked")
        {
            return StubServer.IsGameObjectChecked(obj);
        }
        string message = string.Format("predicate with name={0} is not supported", key);
        throw new ActionError(-32000, message);
    }
}


public class StubServer
{
    private readonly Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
    private readonly List<StubClient> clients = new List<StubClient>();
    private readonly object locker = new object();
    private readonly Queue<KeyValuePair<StubAction, ActionContext>> actions = new Queue<KeyValuePair<StubAction, ActionContext>>();
    private delegate object StubAction(Dictionary<string, object> @params);
    private readonly Dictionary<int, GameObject> cache = new Dictionary<int, GameObject>();
    private readonly Dictionary<string, StubAction> methods;
    private static readonly List<Type> textTypes = new List<Type>();
    public Dictionary<string, object> highlightRect;
    private Texture2D texture;
    private GameObject stubNode;

    public StubServer()
    {
        foreach (string typeName in new string[] { "TextMeshProUGUI", "TextMeshPro", "TMP_InputField" })
        {
            Type t = Type.GetType(string.Format("TMPro.{0},Unity.TextMeshPro.dll", typeName));
            if (t != null)
            {
                textTypes.Add(t);
            }
        }
        textTypes.Add(typeof(Text));
        textTypes.Add(typeof(TextMesh));
        textTypes.Add(typeof(InputField));
        methods = new Dictionary<string, StubAction>();
        methods["get_element_ids"] = GetElementIds;
        methods["get_rect"] = GetGameObjectRect;
        methods["get_text"] = GetGameObjectText;
        methods["set_text"] = SetGameObjectText;
        methods["click"] = ClickGameObject;
        methods["get_version"] = GetVersion;
        methods["get_parent"] = GetGameObjectParent;
        methods["get_children"] = GetGameObjectChildren;
        methods["get_element_info"] = GetGameObjectInfo;
        methods["get_visual_rect"] = GetVisualRect;
        methods["is_visible"] = IsGameObjectVisible;
        methods["is_enabled"] = IsGameObjectEnabled;
        methods["is_checked"] = IsGameObjectChecked;
        methods["get_current_scene_name"] = GetCurrentSceneName;
        methods["get_type"] = GetGameObjectType;
        methods["highlight_rect"] = HighlightRect;
        methods["hide_highlight_rect"] = HideHighlightRect;
        methods["get_ui_tree"] = GetUITree;
        methods["call_method"] = CallMethod;
        methods["load_scene"] = LoadScene;
        methods["get_texture"] = GetGameObjectTexture;
        methods["get_scroll_info"] = GetGameObjectScrollInfo;
        methods["scroll_to_end"] = ScrollGameObjectToEnd;
        methods["get_scene_names"] = GetAllSceneNames;
        methods["is_clickable"] = IsGameObjectClickable;

        Dictionary<string, string> info = GetVersion(null);
        string stubInfo = string.Format("stub_version={0}, unity_version={1}, app_version={2}, app_id={3}", info["stub_version"], info["unity_version"], info["app_version"], info["app_id"]);
        UnityEngine.Debug.Log(string.Format("[SHOOTS_UNITY]stub info: {0}", stubInfo));

        int unityPort = 0;
        string unityPortString = System.Environment.GetEnvironmentVariable("SHOOTS_UNITY_PORT");
        if (unityPortString != null)
        {
            unityPort = int.Parse(unityPortString);
        }
        IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, unityPort);
        s.Bind(iep);
        s.Listen(5);
        s.BeginAccept(new AsyncCallback(OnAccept), s);
        int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
        string portFile = string.Format("{0}.stub_port", pid);
        int port = ((IPEndPoint)s.LocalEndPoint).Port;
        string tempPath;
        if (Application.platform == RuntimePlatform.Android)
        {
            string[] items = { "media", "data" };
            foreach (string item in items)
            {
                string tempDir = string.Format("/sdcard/Android/{0}/{1}/files", item, Application.identifier);
                UnityEngine.Debug.Log(string.Format("[SHOOTS_UNITY]temp dir: {0}", tempDir));
                tempPath = System.IO.Path.Combine(tempDir, portFile);
                try
                {
                    System.IO.Directory.CreateDirectory(tempDir);
                    System.IO.File.WriteAllText(tempPath, port.ToString());
                    UnityEngine.Debug.Log(string.Format("[SHOOTS_UNITY]Write port to {0}", tempPath));
                }
                catch(Exception e)
                {
                    UnityEngine.Debug.Log(string.Format("[SHOOTS_UNITY]Write {0} failed: {1}", tempPath, e.Message));
                }
            }
        }
        else
        {
            tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), portFile);
            System.IO.File.WriteAllText(tempPath, port.ToString());
            UnityEngine.Debug.Log(string.Format("[SHOOTS_UNITY]Write port to {0}", tempPath));
        }
        UnityEngine.Debug.Log(string.Format("[SHOOTS_UNITY]Unity Stub listening to port {0}", port));
    }

    public void SetStubNode(GameObject obj)
    {
        stubNode = obj;
    }

    public void CleanUp()
    {
        cache.Clear();
        highlightRect = null;
    }

    void PushActions(KeyValuePair<StubAction, ActionContext> pair)
    {
        lock (locker)
        {
            actions.Enqueue(pair);
        }
    }

    public void HandleActions()
    {
        lock (locker)
        {
            while (actions.Count > 0)
            {
                KeyValuePair<StubAction, ActionContext> pair = actions.Dequeue();
                StubAction action = pair.Key;
                ActionContext ctx = pair.Value;
                try
                {
                    ctx.result = action(ctx.data);
                }
                catch (ActionError e)
                {
                    Dictionary<string, object> result = new Dictionary<string, object>();
                    result["code"] = e.Code;
                    result["message"] = e.Message;
                    ctx.raised = true;
                    ctx.result = result;
                }
                catch (Exception e)
                {
                    Dictionary<string, object> result = new Dictionary<string, object>();
                    string message = string.Format("{0}\n{1}", e.Message, e.StackTrace);
                    result["code"] = -32000;
                    result["message"] = message;
                    ctx.raised = true;
                    ctx.result = result;
                }
                ctx.handle.Set();
            }
        }
    }

    public void RemoveClient(StubClient client)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i] == client)
            {
                clients.RemoveAt(i);
                break;
            }
        }
    }

    private void OnAccept(IAsyncResult iar)
    {
        Socket s = (Socket)iar.AsyncState;
        Socket client = s.EndAccept(iar);
        clients.Add(new StubClient(client, this));
        s.BeginAccept(new AsyncCallback(OnAccept), s);
    }

    public object HandleRequest(StubRequest req)
    {
        string method = req.method;
        Dictionary<string, object> @params = req.@params;
        StubAction action;
        if (methods.ContainsKey(method))
        {
            action = methods[method];
        }
        else
        {
            throw new Exception(string.Format("method={0} not recognized", method));
        }
        ActionContext ctx = DispatchMainThread(action, @params);
        if (ctx.raised)
        {
            StubErrorResponse rsp = new StubErrorResponse
            {
                id = req.id,
                jsonrpc = req.jsonrpc
            };
            rsp.error = (Dictionary<string, object>)ctx.result;
            return rsp;
        }
        else
        {
            StubResponse rsp = new StubResponse
            {
                id = req.id,
                jsonrpc = req.jsonrpc
            };
            rsp.result = ctx.result;
            return rsp;
        }
    }

    private ActionContext DispatchMainThread(StubAction action, Dictionary<string, object> @params)
    {
        ActionContext ctx = new ActionContext
        {
            data = @params,
            handle = new EventWaitHandle(false, EventResetMode.AutoReset)
        };
        PushActions(new KeyValuePair<StubAction, ActionContext>(action, ctx));
        ctx.handle.WaitOne();
        return ctx;
    }

    private GameObject GetObjectFromHash(int hash)
    {
        GameObject obj = null;
        if (!cache.ContainsKey(hash))
        {
            List<GameObject> roots = new List<GameObject>(GetRootGameObjects());
            while (roots.Count > 0)
            {
                GameObject temp = roots[0];
                if (temp.GetInstanceID() == hash)
                {
                    cache.Add(hash, temp);
                    obj = temp;
                    break;
                }
                roots.RemoveAt(0);
                roots.AddRange(GetGameObjectChildren(temp));
            }
        }
        else
        {
            obj = cache[hash];
        }
        if (obj == null)
        {
            throw new ActionError(1000, string.Format("elem_id={0} is invalid", hash));
        }
        return obj;
    }

    private void CacheGameObject(GameObject obj)
    {
        int hash = obj.GetInstanceID();
        if (!cache.ContainsKey(hash))
        {
            cache.Add(hash, obj);
        }
    }

    private GameObject[] GetRootGameObjects()
    {
        UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        List<GameObject> tempRoots = new List<GameObject>(activeScene.GetRootGameObjects());
        GameObject[] dontDestroyNodes = stubNode.scene.GetRootGameObjects();
        tempRoots.AddRange(dontDestroyNodes);
        return tempRoots.ToArray();
    }

    private object GetElementIds(Dictionary<string, object> @params)
    {
        GameObject[] roots;
        if (@params["root_id"] == null)
        {
            roots = GetRootGameObjects();
        }
        else
        {
            GameObject root = GetObjectFromHash((int)@params["root_id"]);
            roots = GetGameObjectChildren(root).ToArray();
        }
        List<UPathItem> upath = new List<UPathItem>();
        List<object> path = (List<object>)@params["path"];
        foreach (Dictionary<string, object> item in path)
        {
            upath.Add(new UPathItem()
            {
                predicates = (List<object>)item["predicates"],
                index = item["index"],
                depth = item["depth"]
            });
        }
        return GetElementIds(upath, roots);
    }

    private object GetElementIds(List<UPathItem> path, GameObject[] roots)
    {
        List<GameObject> tempRoots = new List<GameObject>();
        List<GameObject> matchedObjects = new List<GameObject>();
        List<GameObject> tempChildren = new List<GameObject>(roots);

        foreach (UPathItem pathItem in path)
        {
            int depth = 1;
            tempRoots.Clear();
            tempRoots.AddRange(tempChildren);
            tempChildren.Clear();
            matchedObjects.Clear();
            while (tempRoots.Count > 0)
            {
                if (pathItem.depth != null && depth > (int)pathItem.depth)
                {
                    break;
                }
                for (int i = 0; i < tempRoots.Count; i++)
                {
                    GameObject tempRoot = tempRoots[i];
                    if (pathItem.IsMatch(tempRoot))
                    {
                        matchedObjects.Add(tempRoot);
                    }
                    tempChildren.AddRange(GetGameObjectChildren(tempRoot));
                }
                depth++;
                tempRoots.Clear();
                tempRoots.AddRange(tempChildren);
                tempChildren.Clear();
                if (pathItem.index != null)
                {
                    // break for efficiency
                    int index = (int)pathItem.index;
                    if (index >= 0 && index < matchedObjects.Count)
                    {
                        break;
                    }
                    else if (index < 0 && -index <= matchedObjects.Count)
                    {
                        break;
                    }

                }
            }
            if (pathItem.index != null)
            {
                int index = (int)pathItem.index;
                if (index >= 0 && index < matchedObjects.Count)
                {
                    matchedObjects = new List<GameObject>() { matchedObjects[index] };
                }
                else if (index < 0 && -index <= matchedObjects.Count)
                {
                    matchedObjects = new List<GameObject>() { matchedObjects[matchedObjects.Count + index] };
                }
                else
                {
                    matchedObjects = new List<GameObject>();
                }
            }

            foreach (GameObject obj in matchedObjects)
            {
                tempChildren.AddRange(GetGameObjectChildren(obj));
            }
        }
        List<int> objectIds = new List<int>();
        foreach (GameObject obj in matchedObjects)
        {
            CacheGameObject(obj);
            objectIds.Add(obj.GetInstanceID());
        }
        return objectIds;
    }

    private Dictionary<string, string> GetVersion(Dictionary<string, object> @params)
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        info["stub_version"] = "0.0.21.0";
        info["unity_version"] = Application.unityVersion;
        info["app_version"] = Application.version;
        info["app_id"] = Application.identifier;
        return info;
    }

    public static string GetGameObjectText(GameObject obj)
    {
        string text = "";
        foreach (Type t in textTypes)
        {
            Component component = obj.GetComponent(t);
            if (component != null)
            {
                text = (string)t.GetProperty("text").GetValue(component, null);
                break;
            }
        }
        return text;
    }

    private object GetGameObjectText(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return GetGameObjectText(obj);
    }
    private object ClickGameObject(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        ExecuteEvents.Execute(obj, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
        return GetGameObjectText(obj);
    }

    private object SetGameObjectText(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        bool bFound = false;
        List<Component> components = new List<Component>();
        List<Type> matchedTypes = new List<Type>();
        List<string> foundedTypes = new List<string>();
        foreach (Type t in textTypes)
        {
            Component[] temp = obj.GetComponents(t);
            if (temp.Length > 0)
            {
                if (!bFound)
                {
                    bFound = true;
                    components.AddRange(temp);
                    matchedTypes.Add(t);
                    foundedTypes.Add(t.ToString());
                }
                else
                {
                    string message = string.Format("multiple text component found: {0}", string.Join(",", foundedTypes.ToArray()));
                    throw new ActionError(-32000, message);
                }
            }
        }
        if (components.Count == 0)
        {
            string message = string.Format("no text component found for {0}", GetGameObjectInfo(obj).ToString());
            throw new ActionError(-32000, message);
        }
        else if (components.Count > 1)
        {
            string message = string.Format("got multiple scomponents for {0}", GetGameObjectInfo(obj).ToString());
            throw new ActionError(-32000, message);
        }
        else
        {
            PropertyInfo p = matchedTypes[matchedTypes.Count - 1].GetProperty("text");
            p.SetValue(components[0], (string)@params["text"], null);
            return null;
        }
    }

    private static GameObject GetGameObjectParent(GameObject obj)
    {
        Transform transformParent = obj.transform.parent;
        if (transformParent == null)
        {
            return null;
        }
        else
        {
            return transformParent.gameObject;
        }
    }

    private object GetGameObjectParent(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        GameObject parent = GetGameObjectParent(obj);
        if (parent != null)
        {
            CacheGameObject(parent);
            return parent.GetInstanceID();
        }
        return null;
    }

    private List<GameObject> GetGameObjectChildren(GameObject obj)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform t in obj.transform)
        {
            children.Add(t.gameObject);
        }
        return children;
    }

    private object GetGameObjectChildren(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        List<GameObject> children = GetGameObjectChildren(obj);
        List<int> childrenIds = new List<int>();
        foreach (GameObject child in children)
        {
            CacheGameObject(child);
            childrenIds.Add(child.GetInstanceID());
        }
        return childrenIds;
    }

    private object GetGameObjectInfo(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return GetGameObjectInfo(obj);
    }

    private object GetGameObjectInfo(GameObject obj)
    {
        Dictionary<string, object> info = new Dictionary<string, object>();
        try
        {
            info["type"] = GetGameObjectType(obj);
            info["tag"] = obj.tag;
            info["name"] = obj.name;
            info["visible"] = IsGameObjectVisible(obj);
            info["enabled"] = IsGameObjectEnabled(obj);
            //info["clickable"] = IsGameObjectClickable(obj);
            info["text"] = GetGameObjectText(obj);
            info["rect"] = GetGameObjectRect(obj);
            info["texture"] = GetGameObjectTexture(obj);
            info["checked"] = IsGameObjectChecked(obj);
            info["z_coordinate"] = obj.transform.position.z;
            info["layer"] = obj.layer;
            if (obj.GetComponent<ScrollRect>() != null)
            {
                info["scroll_info"] = GetGameObjectScrollInfo(obj);
            }
            info["clickable"] = IsGameObjectClickable(obj);
        }
        catch (Exception e)
        {
            string message = string.Format("Get element info failed for {0}:\n{1}{2}", obj, e.Message, e.StackTrace);
            info["error"] = message;
        }
        return info;
    }

    private object GetGameObjectRect(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return GetGameObjectRect(obj);
    }

    private static Vector3 WorldToScreenPoint(Vector3 v, Camera camera)
    {
        return camera.WorldToScreenPoint(v);
    }

    private static Camera GetGameObjectCamera(GameObject obj)
    {
        if (Camera.main != null)
        {
            return Camera.main;
        }
        else
        {
            Camera matched = Camera.main;
            foreach (Camera camera in Camera.allCameras)
            {
                if (camera == Camera.main)
                {
                    continue;
                }
                if ((camera.cullingMask & (1 << obj.layer)) != 0)
                {
                    matched = camera;
                    break;
                }
            }
            return matched;
        }
    }

    private static Rect GetBoundsRect(Bounds bounds, Camera camera)
    {
        if (camera == null)
        {
            return new Rect(0, 0, 0, 0);
        }
        Vector3 cen = bounds.center;
        Vector3 ext = bounds.extents;

        Vector2 min = WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z - ext.z), camera);
        Vector2 max = min;

        //0
        Vector2 point = min;
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //1
        point = WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z - ext.z), camera);
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);


        //2
        point = WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z + ext.z), camera);
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //3
        point = WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z + ext.z), camera);
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //4
        point = WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z - ext.z), camera);
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //5
        point = WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z - ext.z), camera);
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //6
        point = WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z + ext.z), camera);
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //7
        point = WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z + ext.z), camera);
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        return new Rect(min.x, max.y, max.x - min.x, max.y - min.y);
    }

    private static Canvas GetCanvas(GameObject obj)
    {
        GameObject temp = obj;
        while (true)
        {
            Canvas canvas = temp.GetComponent<Canvas>();
            if (canvas != null)
            {
                return canvas;
            }
            GameObject parent = GetGameObjectParent(temp);
            if (parent == null)
            {
                return null;
            }
            else
            {
                temp = parent;
            }
        }
    }

    public static object GetGameObjectRect(GameObject obj)
    {
        Rect rect = InnerGetGameObjectRect(obj);
        Dictionary<string, float> rectInfo = new Dictionary<string, float>();
        rectInfo["left"] = rect.xMin / Screen.width;
        rectInfo["top"] = 1 - rect.yMin / Screen.height;
        rectInfo["width"] = rect.width / Screen.width;
        rectInfo["height"] = rect.height / Screen.height;
        return rectInfo;
    }

    private static Rect InnerGetGameObjectRect(GameObject obj)
    {
        Rect rect;
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return GetBoundsRect(renderer.bounds, GetGameObjectCamera(obj));
        }

        // 1. get rect from RectTransform
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Canvas canvas = GetCanvas(obj);
            float scaleFactor = 1.0f;
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            Bounds bounds = new Bounds(worldCorners[0], Vector3.zero);
            for (int i = 1; i < 4; ++i)
            {
                bounds.Encapsulate(worldCorners[i]);
            }
            RenderMode mode = RenderMode.WorldSpace;
            if (canvas != null)
            {
                mode = canvas.renderMode;
            }
            if (mode == RenderMode.ScreenSpaceOverlay)
            {
                // we should only handle scaleFactor for ScreenSpaceOverlay mode
                scaleFactor = canvas.scaleFactor;
                rect = new Rect(new Vector2(bounds.min.x, bounds.max.y), rectTransform.rect.size);
            }
            else if (mode == RenderMode.ScreenSpaceCamera)
            {
                rect = GetBoundsRect(bounds, canvas.worldCamera);
            }
            else
            {
                Camera camera;
                if (canvas != null && canvas.worldCamera)
                {
                    // using Canvas.worldCamera first
                    camera = canvas.worldCamera;
                }
                else
                {
                    camera = GetGameObjectCamera(obj);
                }
                rect = GetBoundsRect(bounds, camera);
            }
            return new Rect(rect.xMin, rect.yMin, rect.width * scaleFactor, rect.height * scaleFactor);
        }

        // 2. get rect from SkinnedMeshRenderer
        SkinnedMeshRenderer meshRenderer = obj.GetComponent<SkinnedMeshRenderer>();
        if (meshRenderer != null)
        {
            return GetBoundsRect(meshRenderer.bounds, GetGameObjectCamera(obj));
        }

        // 3. get rect for NGUI widget
        //UIWidget widget = obj.GetComponent<UIWidget>();
        //if (widget != null)
        //{
        //    return GetBoundsRect(widget, obj);
        //}

        return new Rect(0, Screen.height, 0, 0);
    }

    private object GetVisualRect(Dictionary<string, object> @params)
    {
        Dictionary<string, float> visualRect = new Dictionary<string, float>();
        visualRect["left"] = 0;
        visualRect["top"] = 0;
        visualRect["width"] = 1;
        visualRect["height"] = 1;
        return visualRect;
    }

    private object IsGameObjectVisible(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return IsGameObjectVisible(obj);
    }

    public static bool IsGameObjectVisible(GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            return false;
        }
        else
        {
            Dictionary<string, float> rectInfo = (Dictionary<string, float>)GetGameObjectRect(obj);
            if (rectInfo["left"] > 1 || rectInfo["top"] > 1)
            {
                return false;
            }
            if (rectInfo["left"] < 0 && (rectInfo["left"] + rectInfo["width"]) < 0)
            {
                return false;
            }
            if (rectInfo["top"] < 0 && (rectInfo["top"] + rectInfo["height"]) < 0)
            {
                return false;
            }
            return true;
        }
    }

    private object IsGameObjectEnabled(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return IsGameObjectEnabled(obj);
    }

    public static bool IsGameObjectEnabled(GameObject obj)
    {
        Selectable selectable = obj.GetComponent<Selectable>();
        if (selectable != null)
        {
            return selectable.interactable;
        }
        return true;
    }

    private object IsGameObjectChecked(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        object result = IsGameObjectChecked(obj);
        if (result == null)
        {
            string message = string.Format("{0} got no toggle component", obj);
            throw new ActionError(-32000, message);
        }
        return result;
    }

    static public object IsGameObjectChecked(GameObject obj)
    {
        Toggle toggle = obj.GetComponent<Toggle>();
        if (toggle != null)
        {
            return toggle.isOn;
        }
        return null;
    }

    private object GetCurrentSceneName(Dictionary<string, object> @params)
    {
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        return currentScene.name;
    }

    private object GetGameObjectType(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return GetGameObjectType(obj);
    }

    public static string GetGameObjectType(GameObject obj)
    {
        string type = obj.GetType().ToString();
        string[] parts = type.Split('.');
        return parts[parts.Length - 1];
    }

    public void ShowHighlight()
    {
        if (highlightRect != null)
        {
            float left = (float)(Screen.width * Convert.ToDouble(highlightRect["left"]));
            float top = (float)(Screen.height * Convert.ToDouble(highlightRect["top"]));
            float width = (float)(Screen.width * Convert.ToDouble(highlightRect["width"]));
            float height = (float)(Screen.height * Convert.ToDouble(highlightRect["height"]));
            GUI.skin.box.normal.background = texture;
            GUI.Box(new Rect(left, top, width, height), GUIContent.none);
        }
    }

    public object HighlightRect(Dictionary<string, object> @params)
    {
        highlightRect = (Dictionary<string, object>)@params["rect"];
        int width = (int)(Screen.width * Convert.ToDouble(highlightRect["width"]));
        int height = (int)(Screen.height * Convert.ToDouble(highlightRect["height"]));

        texture = new Texture2D(width, height);
        Color boarderColor = new Color(1, 0, 0);
        Color contentColor = new Color(0, 0, 0, 0);
        int linePixels = 3;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if ((x >= 0 && x <= linePixels) || (y >= 0 && y <= linePixels))
                {
                    texture.SetPixel(x, y, boarderColor);
                }
                else if ((width - x) >= 0 && (width - x) <= linePixels)
                {
                    texture.SetPixel(x, y, boarderColor);
                }
                else if ((height - y) >= 0 && (height - y) <= linePixels)
                {
                    texture.SetPixel(x, y, boarderColor);
                }
                else
                {
                    texture.SetPixel(x, y, contentColor);
                }
            }
        }
        texture.Apply();
        return true;
    }

    public object HideHighlightRect(Dictionary<string, object> @params)
    {
        highlightRect = null;
        return true;
    }

    public object GetUITree(Dictionary<string, object> @params)
    {
        GameObject[] roots;
        List<Dictionary<string, object>> tree = new List<Dictionary<string, object>>();
        if (@params["root_id"] == null)
        {
            roots = GetRootGameObjects();
        }
        else
        {
            GameObject root = GetObjectFromHash((int)@params["root_id"]);
            roots = new GameObject[] { root };
        }
        foreach (GameObject root in roots)
        {
            Dictionary<string, object> temp_tree = new Dictionary<string, object>();
            List<KeyValuePair<GameObject, object>> temp_nodes = new List<KeyValuePair<GameObject, object>>() {
                new KeyValuePair<GameObject, object>(root, temp_tree)
            };
            while (temp_nodes.Count > 0)
            {
                KeyValuePair<GameObject, object> pair = temp_nodes[0];
                temp_nodes.RemoveAt(0);
                GameObject obj = pair.Key;
                Dictionary<string, object> subTree = (Dictionary<string, object>)pair.Value;
                subTree["id"] = obj.GetInstanceID();
                subTree["elem_info"] = GetGameObjectInfo(obj);
                List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();
                subTree["children"] = children;
                foreach (GameObject child in GetGameObjectChildren(obj))
                {
                    Dictionary<string, object> childTree = new Dictionary<string, object>();
                    children.Add(childTree);
                    temp_nodes.Add(new KeyValuePair<GameObject, object>(child, childTree));
                }
            }
            tree.Add(temp_tree);
        }
        return tree;
    }

    private object CallMethod(Dictionary<string, object> @params)
    {
        Dictionary<string, object> kwargs = (Dictionary<string, object>)@params["kwargs"];
        object target;
        bool isClass = false;
        if (@params.ContainsKey("elem_id") && @params["elem_id"] != null)
        {
            target = GetObjectFromHash((int)@params["elem_id"]);
        }
        else
        {
            if (!kwargs.ContainsKey("class_name"))
            {
                throw new ActionError(-32000, "either elem_id or class_name should be specified");
            }
            string class_name = (string)kwargs["class_name"];
            Type t = Type.GetType(class_name);
            if (t == null)
            {
                throw new ActionError(-32000, string.Format("class_name={0} not found", class_name));
            }
            if (kwargs.ContainsKey("object_id"))
            {
                IntPtr object_id = (IntPtr)(int)kwargs["object_id"];
                GCHandle handle = GCHandle.FromIntPtr(object_id);
                target = handle.Target;
            }
            else
            {
                target = t;  // class method invoked
                isClass = true;
            }
        }
        MethodInfo method;
        string methodName = (string)@params["method"];
        if (!kwargs.ContainsKey("component"))
        {
            if (isClass)
            {
                method = ((Type)target).GetMethod(methodName);
            }
            else
            {
                method = target.GetType().GetMethod(methodName);
            }
        }
        else
        {
            string componentType = (string)kwargs["component"];
            Component component = ((GameObject)target).GetComponent(Type.GetType(componentType));
            if (component != null)
            {
                method = component.GetType().GetMethod(methodName);
                target = component;
            }
            else
            {
                throw new ActionError(-32000, string.Format("component={0} not found", componentType));
            }
        }
        if (method == null)
        {
            throw new ActionError(-32000, string.Format("method={0} not found for {1}", methodName, target));
        }
        List<object> args = (List<object>)@params["args"];
        object result;
        try
        {
            result = method.Invoke(target, args.ToArray());
        }
        catch (Exception e)
        {
            string message = string.Format("call_method failed:{0}{1}", e.Message, e.StackTrace);
            throw new ActionError(-32000, message);
        }

        Dictionary<string, object> ret = new Dictionary<string, object>();
        if (result == null || result is bool || result is int || result is short || result is long ||
            result is string || result is IDictionary || result is IList)
        {
            ret["type"] = "basic";
            ret["value"] = result;
        }
        else
        {
            ret["type"] = result.GetType().ToString();
            GCHandle objHandle = GCHandle.Alloc(result, GCHandleType.WeakTrackResurrection);
            ret["value"] = (int)GCHandle.ToIntPtr(objHandle);
        }
        return ret;
    }

    private object LoadScene(Dictionary<string, object> @params)
    {
        string sceneName = (string)@params["scene_name"];
        UnityEngine.SceneManagement.LoadSceneMode mode = (bool)@params["single_mode"] ? UnityEngine.SceneManagement.LoadSceneMode.Single : UnityEngine.SceneManagement.LoadSceneMode.Additive;
        if ((bool)@params["sync"])
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, mode);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
        }
        return null;
    }

    private object GetGameObjectTexture(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return GetGameObjectTexture(obj);
    }

    public static object GetGameObjectTexture(GameObject obj)
    {
        UnityEngine.UI.Image image = obj.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            if (image.overrideSprite != null)
            {
                return image.overrideSprite.name;
            }
            if (image.sprite != null)
            {
                return image.sprite.name;
            }
        }

        RawImage rawImage = obj.GetComponent<RawImage>();
        if (rawImage != null && rawImage.texture != null)
        {
            return rawImage.texture.name;
        }

        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            return spriteRenderer.sprite.name;
        }

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            if (renderer.material.HasProperty("_MainTex") && renderer.material.mainTexture != null)
            {
                return renderer.material.mainTexture.name;
            }
        }
        return null;
    }

    private object GetGameObjectScrollInfo(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return GetGameObjectScrollInfo(obj);
    }

    public object GetGameObjectScrollInfo(GameObject obj)
    {
        ScrollRect scrollRect = obj.GetComponent<ScrollRect>();
        if (scrollRect == null)
        {
            object elemInfo = JSONWriter.ToJson(GetGameObjectInfo(obj));
            string msg = string.Format("no ScrollRect component found for GameObject: {0}", elemInfo);
            throw new ActionError(-32000, msg);
        }
        Dictionary<string, object> scrollInfo = new Dictionary<string, object>();
        scrollInfo["scrollable_x"] = scrollRect.horizontal;
        scrollInfo["start_x"] = 0;
        scrollInfo["end_x"] = 100;
        scrollInfo["current_x"] = Math.Round(scrollRect.horizontalNormalizedPosition * 100);
        scrollInfo["scrollable_y"] = scrollRect.vertical;
        scrollInfo["start_y"] = 0;
        scrollInfo["end_y"] = 100;
        scrollInfo["current_y"] = 100 - Math.Round(scrollRect.verticalNormalizedPosition * 100);
        return scrollInfo;
    }

    private object ScrollGameObjectToEnd(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        if (obj.GetComponent<ScrollRect>() == null)
        {
            string msg = string.Format("non scrollable object found: {0}", GetGameObjectInfo(obj));
            throw new ActionError(-32000, msg);
        }
        ScrollRect scrollRect = obj.GetComponent<ScrollRect>();
        int directionX = (int)@params["direction_x"];
        int directionY = (int)@params["direction_y"];
        if (directionX != 0 && !scrollRect.horizontal)
        {
            string msg = string.Format("horizontal scrolling disabled for: {0}", JSONWriter.ToJson(GetGameObjectInfo(obj)));
            throw new ActionError(-32000, msg);
        }
        if (directionX == 1)
        {
            scrollRect.horizontalNormalizedPosition = 1;
        }
        else if (directionX == -1)
        {
            scrollRect.horizontalNormalizedPosition = 0;
        }

        if (directionY != 0 && !scrollRect.vertical)
        {
            string msg = string.Format("vertical scrolling disabled for: {0}", JSONWriter.ToJson(GetGameObjectInfo(obj)));
            throw new ActionError(-32000, msg);
        }
        if (directionY == 1)
        {
            scrollRect.verticalNormalizedPosition = 0;  // vertical is 0 for bottom
        }
        else if (directionY == -1)
        {
            scrollRect.verticalNormalizedPosition = 1;  // vertical is 1 for top
        }
        // onValueChanged event is automatically fired, don't worry about it!
        return true;
    }

    private object GetAllSceneNames(Dictionary<string, object> @params)
    {
        List<string> names = new List<string>();
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            names.Add(sceneName);
        }
        return names;
    }

    private object IsGameObjectClickable(Dictionary<string, object> @params)
    {
        GameObject obj = GetObjectFromHash((int)@params["elem_id"]);
        return IsGameObjectClickable(obj);
    }

    private bool IsGameObjectClickable(GameObject obj)
    {
        bool raycastTarget = false;
        foreach (Component component in obj.GetComponents<Component>())
        {
            PropertyInfo p = component.GetType().GetProperty("raycastTarget");
            if (p == null)
            {
                continue;
            }
            raycastTarget = (bool)p.GetValue(component);
            if (raycastTarget)
            {
                break;
            }
        }
        return raycastTarget;
    }
}
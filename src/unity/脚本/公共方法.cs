/// <summary>
///    公共方法类
//
/// date: 2015-5-3
/// </summary>

static public class GameUtils
{
    /// <summary>
    /// 两点距离
    /// </summary>
    static public float GetDistance(Vector3 from, Vector3 to)
    {
        from.y = 0;
        to.y = 0;
        return Vector3.Distance(from, to);
    }

    /// <summary>
    /// 获取两点的方向向量
    /// </summary>
    static public Vector3 GetDirect(Vector3 from, Vector3 to)
    {
        from.y = 0;
        to.y = 0;

        Vector3 temp = to - from;
        temp.Normalize();
        return temp;
    }

    /// <summary>
    /// 左右方向
    /// </summary>
    static public Vector3 Get2DDirect(Vector3 from, Vector3 to)
    {
        Vector3 temp = to - from;
        if (Vector3.Dot(CameraMgr.Instance.transform.right, temp.normalized) > 0)
            return CameraMgr.Instance.transform.right;
        else
            return -CameraMgr.Instance.transform.right;
    }

    /// <summary>
    /// 左右方向
    /// </summary>
    static public Vector3 DirectTo2D(Vector3 dir)
    {
        if (Vector3.Dot(CameraMgr.Instance.transform.right, dir.normalized) > 0)
            return CameraMgr.Instance.transform.right;
        else
            return -CameraMgr.Instance.transform.right;
    }

    /// <summary>
    /// 获取对象的指定挂点
    /// </summary>
    static public bool GetHangPoint(GameObject src, string name, out Transform parent)
    {
        Transform[] trans = src.GetComponentsInChildren<Transform>();
        for (int i = trans.Length - 1; i >= 0; --i)
        {
            if (trans[i].name == name)
            {
                parent = trans[i];
                return true;
            }
        }

        parent = src.transform;
        return false;
    }

    /// <summary>
    /// 可触碰目标的距离
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    static public float GetRoleTouchDistance(CharacterBase origin, CharacterBase target)
    {
        if (origin == null || target == null)
            return 99999;

        return GameUtils.GetDistance(origin.transform.position, target.transform.position); //- target.Radius;
    }

    /// <summary>
    /// 专用于技能施放时自动跑向目标的检测
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    static public bool CheckAutoRunDistance(CharacterBase origin, CharacterBase target, float skillDis)
    {
        if (origin == null || target == null)
            return true;

        float dis = GameUtils.GetDistance(origin.transform.position, target.transform.position); //- target.Radius;
        float rad = origin.Radius + target.Radius;
        //MyLog.EditorLog("origin.Radius=" + origin.Radius + ",target.Radius=" + target.Radius);
        //MyLog.EditorLog("skillDis=" + skillDis);

        if (skillDis > 2 && skillDis < 5) //对近战技能距离判定作优化
        {
            if (skillDis > rad)
                skillDis = rad;
            if (skillDis < 2)
                skillDis = 2;
        }
        return dis >= skillDis;
    }

    /// <summary>
    /// 获取怪物ID
    /// </summary>
    static public int GetSourceID(List<IntAttr> attrList)
    {
        IntAttr attr = attrList.Find(p => p.type == (int)ENTITY_ATTR_ID.ENTITY_ATTR_RESID);
        if (attr != null)
        {
            return attr.value;
        }

        return 0;
    }

    /// <summary>
    /// 获取模型资源
    /// </summary>
    static public int GetResID(List<IntAttr> attrList)
    {
        IntAttr job = attrList.Find(p => p.type == (int)PLAYER_ATTR_ID.PLAYER_ATTR_JOB);
        IntAttr camp = attrList.Find(p => p.type == (int)ENTITY_ATTR_ID.ENTITY_ATTR_CAMPID);
        IntAttr order = attrList.Find(p => p.type == (int)ENTITY_ATTR_ID.ENTITY_ATTR_ORDER);
        if (job != null && camp != null && order != null)
        {
            return GetResID(job.value, camp.value, order.value);
        }

        return 0;
    }

    /// <summary>
    /// 获取模型资源
    /// </summary>
    static public int GetResID(int job, int camp, int order)
    {
        int index = camp * 10000 + job * 100 + order;
        RoleAdvancedTable table = TableManager.GetTableData((int)TableID.RoleAdvancedTableID, index) as RoleAdvancedTable;
        if (table != null)
            return table.ModelId;
        else
            return camp * 10000 + job * 100;
    }

    /// <summary>
    /// 获取职业
    /// </summary>
    static public int GetProfession(List<IntAttr> attrList)
    {
        IntAttr job = attrList.Find(p => p.type == (int)PLAYER_ATTR_ID.PLAYER_ATTR_JOB);
        IntAttr camp = attrList.Find(p => p.type == (int)ENTITY_ATTR_ID.ENTITY_ATTR_CAMPID);
        if (job != null && camp != null)
        {
            return GetProfession(job.value, camp.value);
        }

        return 0;
    }

    /// <summary>
    /// 获取职业
    /// </summary>
    static public int GetProfession(int job, int camp)
    {
        return camp * 100 + job;
    }

    /// <summary>
    /// 获得等级
    /// </summary>
    static public int GetLevel(List<IntAttr> attrList)
    {
        IntAttr attr = attrList.Find(p => p.type == (int)ENTITY_ATTR_ID.ENTITY_ATTR_LEVEL);
        if (attr != null)
        {
            return attr.value;
        }

        return 0;
    }

    /// <summary>
    /// 删除所有子物体 author(wxh)
    /// </summary>
    static public void DelAllChild(GameObject parentTarget)
    {
        if (parentTarget == null || parentTarget.transform == null)
            return;

        Transform TF = null;
        int i = 0;
        for (i = 0; i < parentTarget.transform.childCount; i++)
        {
            TF = parentTarget.transform.GetChild(i);
            TF.gameObject.SetActive(false);
            Object.DestroyObject(TF.gameObject);
        }

        parentTarget.transform.DetachChildren();
    }

    /// <summary>
    /// 删除下标从 author(wxh)
    /// </summary>
    static public void DelFromToInedx(GameObject parentTarget, int from, int to)
    {
        if (parentTarget == null || parentTarget.transform == null)
            return;

        Transform TF = null;
        int i = 0;
        List<Transform> list = new List<Transform>(); ;
        for (i = from; i < to; i++)
        {
            TF = parentTarget.transform.GetChild(i);
            TF.gameObject.SetActive(false);
            list.Add(TF);
            Object.DestroyObject(TF.gameObject);
        }
        if (list.Count > 0)
        {
            for (i = 0; i < list.Count; i++)
            {
                Object.DestroyObject(list[i].gameObject);
            }
        }
        parentTarget.transform.DetachChildren();
    }

    /// <summary>
    /// 时间转换 author(wxh) 12:00:00
    /// </summary>
    static public string FormatTime(int t)
    {
        int hour = t / 3600;
        int min = (t % 3600) / 60;
        int sec = t % 60;
        return System.String.Format("{0:D2}:{1:D2}:{2:D2}", hour, min, sec);
    }

    static public string FormatFashionTime(int t)
    {
        int day = t / 86400;
        int hour = (t % 86400) / 3600;
        int min = ((t % 86400) % 3600) / 60;
        if (day >= 0)
        {
            return string.Format(StringConst.FASHION_DAY_HOUR, day, hour);
        }
        else
        {
            if (hour >= 0)
            {
                return string.Format(StringConst.FASHION_HOUR, hour);
            }
            else
            {
                return string.Format(StringConst.FASHION_MINUTE, min);
            }
        }
    }

    static public string FormatLimittActivityTime(int t)
    {
        int day = t / 86400;
        int hour = (t % 86400) / 3600;
        int min = ((t % 86400) % 3600) / 60;
        return string.Format(StringConst.lIMIT_ACTIVITY_TIME, day, hour, min);
    }

    static public string FashionTableTime(int t)
    {
        if (t == -1)
        {
            return StringConst.ACHIEVE_LONG;
        }
        else
        {
            if (t >= 24)
            {
                return string.Format(StringConst.FASHION_DAY, t / 24);
            }
            else
            {
                return string.Format(StringConst.FASHION_HOUR, t % 24);
            }
        }

    }

    /// <summary>
    /// 格式化Buff时间
    /// </summary>
    static public string FormatBuffTime(float t)
    {
        int hour = (int)t / 3600;
        int min = (int)(t % 3600) / 60;
        int sec = (int)t % 60;
        if (hour > 0)
        {
            return hour.ToString() + StringConst.TIME_HOUR;
        }
        else if (min > 0)
        {
            return min.ToString() + StringConst.TIME_MINUTE;
        }
        else
        {
            return sec.ToString() + StringConst.TIME_SECOND;
        }
    }

    static public string FormatLoginWaitTime(float t)
    {
        int hour = (int)t / 3600;
        int min = (int)(t % 3600) / 60;
        int sec = (int)t % 60;
        if (hour > 0)
        {
            return hour.ToString() + StringConst.TIME_HOUR + min.ToString() + StringConst.TIME_MINUTE + sec.ToString() + StringConst.TIME_SECOND;
        }
        else if (min > 0)
        {
            return min.ToString() + StringConst.TIME_MINUTE + sec.ToString() + StringConst.TIME_SECOND;
        }
        else
        {
            return sec.ToString() + StringConst.TIME_SECOND;
        }
    }

    /// <summary>
    /// 时间转换 author(wxh) 2016-01-01
    /// </summary>
    static public string dateTime(int second)
    {
        return new DateTime(1970, 01, 01, 8, 0, 0).AddSeconds(second).ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 详细的时间 2016-01-01-24:00:00
    /// </summary>
    static public string TimeDetail(int second)
    {
        return new DateTime(1970, 01, 01, 8, 0, 0).AddSeconds(second).ToString("yyyy-MM-dd-HH:mm:ss");
    }

    /// <summary>
    /// 0变00
    /// </summary>
    static public string OneToDouble(int num)
    {
        if (num < 10)
        {
            return string.Format("0{0}", num);
        }
        else
        {
            return string.Format("{0}", num);
        }
    }

    /// <summary>
    /// 服务器时间与传入时间的差值
    /// </summary>
    static public int SubServerTime(int second)
    {
        return (int)(TimerMgr.ServerTime - second);
    }

    /// <summary>
    /// 取得服务器时间
    /// </summary>
    static public DateTime GetServerDate()
    {
        return new DateTime(1970, 01, 01, 8, 0, 0).AddSeconds(TimerMgr.ServerTime);
    }

    /// <summary>
    /// 克隆物体 author(wxh)
    /// </summary>
    static public GameObject CloneGameObj(GameObject cloneTarget)
    {
        if (cloneTarget == null) return null;
        GameObject newGameObj = GameObject.Instantiate(cloneTarget) as GameObject;
        return newGameObj;
    }

    /// <summary>
    /// 判断人物是否在摄像机范围内
    /// </summary>
    static public bool CharIsInCameraRange(CharacterBase c)
    {
        Rect screenrct = new Rect(0, 0, Screen.width, Screen.height);
        Vector3 v = Vector3.zero;
        if (CameraMgr.Instance != null)
        {
            v = CameraMgr.Instance.gameObject.GetComponent<Camera>().WorldToScreenPoint(c.transform.position);
        }
        else
        {
            return false;
        }

        return screenrct.Contains(v);

    }

    /// <summary>
    /// 怪物和角色间是否有阻挡
    /// </summary>
    static public bool IsBlockInPlayerMonster(CharacterBase player, CharacterBase monster, float dis = 0)
    {
        Vector3 dir;
        float disPlayermonster = 0;
        Vector3 v1 = new Vector3(player.transform.position.x, player.transform.position.y + player.Height, player.transform.position.z);//player
        Vector3 v2 = new Vector3(monster.transform.position.x, monster.transform.position.y + monster.Height, monster.transform.position.z);//enemy
        //int layermask = 1 << GameLayers.FenceMask | 1 << GameLayers.FloorMask | 1 << GameLayers.ObstaclesMask
        //| 1 << GameLayers.AirWallMask | 1 << GameLayers.SceneMask;
        int layermask = 1 << GameLayers.FenceMask | 1 << GameLayers.FloorMask | 1 << GameLayers.ObstaclesMask | 1 << GameLayers.SceneMask;

        dir = Vector3.Normalize(v2 - v1);
        if (dis == 0)
        {
            disPlayermonster = GameUtils.GetDistance(v1, v2);
        }
        else
        {
            disPlayermonster = dis;
        }
        //RaycastHit hitInfo;
        if (disPlayermonster > 1 && Physics.Raycast(v1, dir, disPlayermonster, layermask))
        {
            //MyLog.EditorLog("hitInfo=" + hitInfo.collider);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 怪物是否在角色的左右一个范围内
    /// </summary>
    static public bool IsMonsterInPlayerAngle(CharacterBase player, CharacterBase monster, float angle)
    {
        float cosangle = Mathf.Cos(angle * Mathf.Deg2Rad);

        Vector3 dir = monster.transform.position - player.transform.position;
        dir.Normalize();
        Vector2 Dir2D = new Vector2(dir.x, dir.z);
        Vector2 play2Ddir = new Vector2(player.transform.forward.x, player.transform.forward.z);
        float result = Vector2.Dot(Dir2D, play2Ddir);// 求夹角

        if (result >= cosangle && result > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    ///  求夹角,为了显示怪物ui的箭头，世界坐标
    /// </summary>
    static public float GetAngle(CharacterBase player, CharacterBase monster)
    {
        float result;
        Vector3 p1 = monster.transform.position;
        Vector3 p2 = player.transform.position;
        p1.y = 0;
        p2.y = 0;


        Vector3 dir = p1 - p2;
        dir.Normalize();
        Vector2 Dir2D = new Vector2(dir.x, dir.z);

        Vector2 camera2Ddir = new Vector2(CameraMgr.Instance.transform.forward.x, CameraMgr.Instance.transform.forward.z);
        if (Vector3.Dot(CameraMgr.Instance.transform.right, dir) > 0)//右边
        {
            result = -Mathf.Acos(Vector2.Dot(Dir2D, camera2Ddir)) / Mathf.Deg2Rad;// 求夹角
        }
        else//左边
        {
            result = Mathf.Acos(Vector2.Dot(Dir2D, camera2Ddir)) / Mathf.Deg2Rad;// 求夹角
        }
        return result;
    }

    /// <summary>
    /// 是否在时间段内
    /// </summary>
    static public bool TimeIsInRange(float currenttime, float starttime, float endtime)
    {
        if (currenttime >= starttime && currenttime < endtime)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// #ffccdd转成Color
    /// </summary>
    static public Color HtmlToColor(string html)
    {
        int red, green, blue = 0;
        char[] rgb;
        html = html.TrimStart('#');
        html = Regex.Replace(html.ToLower(), "[g-zG-Z]", "");

        rgb = html.ToCharArray();
        red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
        green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
        blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);

        Color color = new Color(red / 255f, green / 255f, blue / 255f);
        return color;
    }

    /// <summary>
    /// Color转换成16进制
    /// </summary>
    static public string ColorTo16(Color color)
    {
        string result = "";
        string r = Convert.ToString((int)(color.r * 255), 16);
        string g = Convert.ToString((int)(color.g * 255), 16);
        string b = Convert.ToString((int)(color.b * 255), 16);

        if (r.Length == 1)
        {
            r = "0" + r;
        }

        if (g.Length == 1)
        {
            g = "0" + g;
        }

        if (b.Length == 1)
        {
            b = "0" + b;
        }

        result = r + g + b;
        return result;
    }

    /// <summary>
    /// 是否是player召唤的monster
    /// </summary>
    static public bool MonsterOwerIsLocalPlayer(long pid)
    {
        CharacterBase charbse = GameRemoteController.Instance.GetCharacterByPID(pid, CharacterType.Monster);
        if (charbse != null && charbse.ownpid == LocalPlayer.Instance.pid)
            return true;

        charbse = GameRemoteController.Instance.GetCharacterByPID(pid, CharacterType.Servant);
        if (charbse != null && charbse.ownpid == LocalPlayer.Instance.pid)
            return true;

        return false;
    }

    /// <summary>
    /// 判断配置表的专精和人物的专精是否匹配
    /// </summary>
    static public bool IsMasteryMatch(int tableMastery, int playerMastery)
    {
        int i = 1 << playerMastery;
        if ((tableMastery & i) > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 判断该功能是否开放
    /// </summary>
    /// <param name="level">等级需求</param>
    static public bool IsFunctionOpen(int level)
    {
        if (LocalPlayer.Instance.Attribute.Level >= level)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 判断技能是否达到最高等级
    /// </summary>
    /// <param name="skill">等级需求</param>
    static public bool IsSkillMaxLevel(SkillTable skill)
    {
        if (LocalPlayer.Instance.SkillVOList.ContainsKey(skill.skill_id))
        {
            if (LocalPlayer.Instance.SkillVOList[skill.skill_id].SkillLevel >= skill.max_level)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取头像
    /// </summary>
    static public int GetHeadIcon(int campId, int job, CharacterType type)
    {
        int tableId = campId * 100 + job;
        return GetHeadIcon(tableId, type);
    }

    /// <summary>
    /// 获取头像
    /// </summary>
    static public int GetHeadIcon(int jobid, CharacterType type)
    {
        int id = 0;
        switch (type)
        {
            case CharacterType.OtherPlayer:
            case CharacterType.Player:
                NewCharacterTable table = TableManager.GetTableData((int)TableID.NewCharacterTableID, jobid) as NewCharacterTable;
                if (table != null)
                {
                    id = table.IconId;
                }
                break;
            case CharacterType.Monster:
                MonsterTable m_monsterInfo = null;
                m_monsterInfo = TableManager.GetTableData((int)TableID.MonsterTableID, jobid) as MonsterTable;
                if (m_monsterInfo != null)
                {
                    id = m_monsterInfo.nHeadIcon;
                }
                break;
            case CharacterType.NPC:
                NpcListTable m_info = TableManager.GetTableData((int)TableID.NpcListTableID, jobid) as NpcListTable;
                if (m_info != null)
                {
                    id = m_info.IconId;
                }
                break;
            case CharacterType.Servant:
                MercenaryTable sInfo = TableManager.GetTableData((int)TableID.MercenaryTableID, jobid) as MercenaryTable;
                if (sInfo != null)
                {
                    id = sInfo.nIcon;
                }
                break;
            default:
                break;
        }
        return id;
    }

    /// <summary>
    /// 金币是否足够
    /// </summary>
    static public bool PlayerGoldIsEnough(int need)
    {
        if (LocalPlayer.Instance != null)
        {
            return LocalPlayer.Instance.Attribute.Gold >= need;
        }
        return false;
    }

    /// <summary>
    /// 元宝是否足够
    /// </summary>
    static public bool PlayerJewelIsEnough(int need)
    {
        if (LocalPlayer.Instance != null)
        {
            return LocalPlayer.Instance.Attribute.Jewel >= need;
        }
        return false;
    }

    /// <summary>
    /// 玩家金钱是否充足
    /// </summary>
    static public bool PlayerMoneyIsEnough(GameCoin type, int need)
    {
        bool result = false;

        if (LocalPlayer.Instance != null)
        {
            switch (type)
            {
                case GameCoin.Gold: result = LocalPlayer.Instance.Attribute.Gold >= need; break;
                case GameCoin.Diamond: result = LocalPlayer.Instance.Attribute.Jewel >= need; break;
                case GameCoin.Crystal: result = LocalPlayer.Instance.Attribute.Crystal >= need; break;
            }
        }

        return result;
    }

    /// <summary>
    /// 提示金币不足，TODO：需要跳转到充值界面等功能
    /// </summary>
    static public void TipsForGoldNotEnough(bool isgold)
    {
        string str;
        string comfirm = StringConst.COMMON_CONFIRM;
        string cancel = StringConst.COMMON_CANCEL;
        string title = StringConst.COMMON_WARN;
        if (isgold)
        {
            NoticeMgr.Instance.GetNoticeString(7001, out str);
        }
        else
        {
            NoticeMgr.Instance.GetNoticeString(7002, out str);
        }
        WindowMgr.OpenConfirmWindow(comfirm, cancel, title, str, null, null);
    }

    /// <summary>
    /// 获得品质字符串
    /// </summary>
    static public string GetQualityString(int id)
    {
        if (id == 0)
            id = 1;
        string str = Equipment.EQUIP_COLOR[id] + (TableManager.GetTableData((int)TableID.QualityTableID, id) as QualityTable).qualitydes + "[-]";

        return str;
    }

    /// <summary>
    /// 得到坐骑在坐骑表里的index id
    /// </summary>
    static public int GetMountId(int type, int quality)
    {
        return type * 10000 + quality;
    }

    /// <summary>
    /// 坐骑品质属性表id
    /// </summary>
    static public int GetMountAttrId(int type, int quality)
    {
        return type * 1000 + quality;
    }

    static public int GetMountAttrLevel(int level)
    {
        return level * 100;
    }

    /// <summary>
    /// 获得场景里GameObject的完整路径
    /// </summary>
    static public string GetFullName(Transform trans, Transform endTrans = null)
    {
        string fullName = trans.name;

        while (trans.parent != null && (endTrans == null || endTrans != trans))
        {
            trans = trans.parent;
            fullName = trans.name + "/" + fullName;
        }

        return fullName;
    }

    static public string GetFullNameWinParOrClone(Transform trans)
    {
        string fullName = trans.name;

        while (trans.parent != null && trans.GetComponent<WindowBase>() == null && (!trans.name.EndsWith("(Clone)") || trans.name == fullName))
        {
            trans = trans.parent;
            fullName = trans.name + "/" + fullName;
        }

        return fullName;
    }

    /// <summary>
    /// 字符串转int数组
    /// </summary>
    static public int[] String2ArrayInt(string str, string strSign)
    {
        string[] strArr = str.Split(new string[] { strSign }, System.StringSplitOptions.None);
        int[] ret = new int[strArr.Length];
        for (int i = 0; i < strArr.Length; i++)
        {
            ret[i] = int.Parse(strArr[i]);
        }

        return ret;
    }

    /// <summary>
    /// 字符串转float数组
    /// </summary>
    static public float[] String2ArrayFloat(string str, string strSign)
    {
        string[] strArr = str.Split(new string[] { strSign }, System.StringSplitOptions.None);
        float[] ret = new float[strArr.Length];
        for (int i = 0; i < strArr.Length; i++)
        {
            ret[i] = float.Parse(strArr[i]);
        }

        return ret;
    }

    /// <summary>
    /// 字符串按分隔符拆分后转换为Vector3
    /// </summary>
    static public Vector3 String2Pos(string str, string strSign)
    {
        Vector3 pos = Vector3.zero;
        float[] posXYZ = String2ArrayFloat(str, strSign);
        pos.x = posXYZ[0];
        pos.y = posXYZ[1];
        if (posXYZ.Length > 2)
        {
            pos.z = posXYZ[2];
        }
        else
        {
            pos.z = 0;
        }
        return pos;
    }

    /// <summary>
    /// 检测字符串内是否有除字母，数字，中文外的其他字符
    /// </summary>
    static public bool CheckHasSymbol(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            char c = Convert.ToChar(word[i]);

            //正则检测是否是字母，数字，中文
            Regex rx = new Regex("^[a-zA-Z0-9\u4e00-\u9fa5]$");
            if (!rx.IsMatch(word[i].ToString()))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 检测字符串长度
    /// </summary>
    /// <param name="length">最大长度，中文=2个长度</param>
    static public bool CheckStringLength(string word, int length)
    {
        int count = 0;
        Regex rx = new Regex("^[\u4e00-\u9fa5]$");
        for (int i = 0; i < word.Length; i++)
        {
            if (rx.IsMatch(word[i].ToString()))
            {
                count += 2;
            }
            else
            {
                count += 1;

            }
        }

        return count <= length;
    }


    /// <summary>
    /// 设置标签按钮图片的点击状态 约定图片名字相同，尾数为2表示点击状态，为1表示非点击状态
    /// </summary>
    static public void SetClickState(UISprite sp, bool isClick)
    {
        string spName = sp.spriteName.Substring(0, sp.spriteName.Length - 1);
        string replaceStr = sp.spriteName.Substring(sp.spriteName.Length - 1, 1);
        if (isClick)
        {
            sp.spriteName = spName + replaceStr.Replace("1", "2");
        }
        else
        {
            sp.spriteName = spName + replaceStr.Replace("2", "1");
        }
    }

    /// <summary>
    /// 设置按钮的置灰
    /// </summary>
    static public void SetBtnGray(UISprite targetSprite, bool toGray)
    {
        GrayUIExchangeMgr.Instance.ChangeUISpriteToGray(targetSprite, toGray);
        targetSprite.GetComponent<BoxCollider>().enabled = !toGray;
    }

    /// <summary>
    /// 清除所有人的头顶连杀图标
    /// </summary>
    static public void ClearAllHudIcon()
    {
        if (GameCoreController.Instance == null || LocalPlayer.Instance == null)
            return;
        List<CharacterBase> roles = GameCoreController.Instance.GetAllOtherPlayers();

        for (int i = 0; i < roles.Count; i++)
            (roles[i] as OtherPlayer).HudSetMultiKillIcon(0);
        LocalPlayer.Instance.HudSetMultiKillIcon(0);
    }

    /// <summary>
    /// 获取随从品质表
    /// </summary>
    static public int GetServantProply(int id, int nQuality)
    {
        return id * 100 + nQuality;
    }

    /// <summary>
    /// 获取随从类型
    /// </summary>
    static public int GetServantType(int id)
    {

        return id % 100000000;
    }

    /// <summary>
    /// 获取随从职业专精表id
    /// </summary>
    static public int GetServntJobTb(int job, int Spcei)
    {
        return job * 100 + Spcei;
    }

    /// <summary>
    /// 获取随从属性表
    /// </summary>
    static public int GetServantAtrrID(int nMercenaryAttrId, int star, int level)
    {

        return nMercenaryAttrId * 1000 + level;
    }

    /// <summary>
    /// 根据随从品质返回对应的颜色
    /// </summary>
    static public Color GetColor(int colorIndex)
    {
        Color servantColor = new Color(); ;
        switch (colorIndex)
        {
            case 1:
                servantColor = HtmlToColor("f1f1f1");
                break;
            case 2:
                servantColor = HtmlToColor("41e914");
                break;
            case 3:
                servantColor = HtmlToColor("17beea");
                break;
            case 4:
                servantColor = HtmlToColor("c613ec");
                break;
            case 5:
                servantColor = HtmlToColor("f09a14");
                break;

        }
        return servantColor;
    }

    /// <summary>
    /// 获得两点之间的等分点
    /// </summary>
    static public List<Vector3> GetBetweenPoints(Vector3 startPos, Vector3 endPos, int count)
    {
        if (count <= 0) return null;

        List<Vector3> posList = new List<Vector3>();
        Vector3 seVec = endPos - startPos;
        for (int i = 0; i < count; i++)
        {
            Vector3 vec = startPos + ((float)(i + 1) / (count + 1)) * seVec;
            posList.Add(vec);
        }

        return posList;
    }

    /// <summary>
    /// 大数显示，大于一万的，显示xx.x万
    /// </summary>
    static public string NumberToString(int num)
    {
        if (num > 10000)
            return string.Format("{0:#.#}{1}", (float)num / 10000.0f, StringConst.COMMON_TNE_THOUSAND);
        else
            return num.ToString();
    }

    /// <summary>
    ///buff排序
    /// </summary>
    static public int SortBuff(Buffer a, Buffer b)
    {

        if (a.BuffTable.show_type < b.BuffTable.show_type)
            return -1;
        else if (a.BuffTable.show_type > b.BuffTable.show_type)
            return 1;
        else
        {
            if (a.BuffBirth > b.BuffBirth)
                return -1;
            else if (a.BuffBirth < b.BuffBirth)
                return 1;
            else
                return 0;
        }
    }

    /// <summary>
    /// 根据品质来排序
    /// </summary>
    static public int SortQuality(ServantData x, ServantData y)
    {
        if ((int)x.ServerState > (int)y.ServerState)
        {
            return -1;
        }
        else if ((int)x.ServerState == (int)y.ServerState)
        {
            if (x.ServantTb.rarity > y.ServantTb.rarity)
            {
                return -1;
            }
            else if (x.ServantTb.rarity == y.ServantTb.rarity)
            {
                if (x.ServantTb.nMercenaryId < y.ServantTb.nMercenaryId)
                {
                    return -1;
                }
                else if (x.ServantTb.nMercenaryId < y.ServantTb.nMercenaryId)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 1;
        }



    }

    /// <summary>
    /// 设置模型Texture尺寸
    /// </summary>
    static public int SetModelTextureSize(int joyType)
    {
        int size = 0;
        switch (joyType)
        {
            case 101:
                size = 528;
                break;
            case 102:
                size = 607;
                break;
            case 104:
                size = 528;
                break;
            case 105:
                size = 530;
                break;
            case 107:
                size = 815;
                break;
            case 201:
                size = 785;
                break;
            case 202:
                size = 624;
                break;
            case 203:
                size = 779;
                break;
            case 205:
                size = 827;
                break;
            case 207:
                size = 743;
                break;
        }
        return size;
    }

    /// <summary>
    /// 设置模型Texture位置
    /// </summary>
    static public Vector3 SetModelTexturePos(int joyType)
    {
        Vector3 pos = Vector3.zero;
        switch (joyType)
        {
            case 101:
                pos = new Vector3(41.82f, -23.38f, 0);
                break;
            case 102:
                pos = new Vector3(41.82f, -23.38f, 0);
                break;
            case 104:
                pos = new Vector3(49.1f, -26.15f, 0);
                break;
            case 105:
                pos = new Vector3(49.1f, -26f, 0);
                break;
            case 107:
                pos = new Vector3(49.1f, 38.13f, 0);
                break;
            case 201:
                pos = new Vector3(56.23f, 9.55f, 0);
                break;
            case 202:
                pos = new Vector3(43.2f, 2.9f, 0);
                break;
            case 203:
                pos = new Vector3(56.23f, 13.93f, 0);
                break;
            case 205:
                pos = new Vector3(56.23f, 22f, 0);
                break;
            case 207:
                pos = new Vector3(56.23f, 13.93f, 0);
                break;
        }
        return pos;
    }

    /// <summary>
    /// 获得随从品阶中文
    /// </summary>
    static public String GetServantQualityOrder(int quality)
    {
        string s = "";
        switch (quality)
        {
            case 1:
                s = StringConst.SERVANT_QUALITY_WHITE;
                break;
            case 2:
                s = StringConst.SERVANT_QUALITY_GREEN;
                break;
            case 3:
                s = StringConst.SERVANT_QUALITY_BLUE;
                break;
            case 4:
                s = StringConst.SERVANT_QUALITY_POPPON; ;
                break;
            case 5:
                s = StringConst.SERVANT_QUALITY_YELLOW;
                break;
        }
        return s;
    }

    /// <summary>
    /// 获得随从技能中文
    /// </summary>
    static public String GetServantSkillLock(int quality)
    {
        string s = "";
        switch (quality)
        {
            case 1:
                s = string.Format(StringConst.SERVANT_SERVANT_QUALITY, StringConst.SERVANT_QUALITY_WHITE);
                break;
            case 2:
                s = string.Format(StringConst.SERVANT_SERVANT_QUALITY, StringConst.SERVANT_QUALITY_GREEN);
                break;
            case 3:
                s = string.Format(StringConst.SERVANT_SERVANT_QUALITY, StringConst.SERVANT_QUALITY_BLUE);
                break;
            case 4:
                s = string.Format(StringConst.SERVANT_SERVANT_QUALITY, StringConst.SERVANT_QUALITY_POPPON);
                break;
            case 5:
                s = string.Format(StringConst.SERVANT_SERVANT_QUALITY, StringConst.SERVANT_QUALITY_YELLOW);
                break;
        }
        return s;
    }

    /// <summary>
    /// 获得随从专精
    /// </summary>
    static public string GetServantMastery(int mastery)
    {
        string s = "";
        switch (mastery)
        {
            case 1:
                s = "tanke";
                break;
            case 2:
                s = "zhiliao";
                break;
            case 3:
                s = "shanghaishuchu";
                break;
        }
        return s;
    }

    /// <summary>
    /// 获取物品提示框
    /// </summary>
    /// <param name="parent">Item的父物体</param>
    /// <param name="localPos">Item的本地位置</param>
    /// <param name="baseDepth">Item要求的最低深度</param>
    /// <returns></returns>
    static private LegacyBottomBaseItem GetTipBlockItem(GameObject parent, Vector3 localPos, int baseDepth, bool showSelect)
    {
        ResourceID id = ResourceID.Block_Block;
        if (showSelect) id = ResourceID.Block_SelectBlock;
        GameObject itemGo = ResourceMgr.LoadAsset(id, typeof(GameObject), true) as GameObject;
        itemGo = NGUITools.AddChild(parent, itemGo);
        itemGo.transform.localPosition = localPos;
        itemGo.SetActive(true);

        LegacyBottomBaseItem item = itemGo.GetComponent<LegacyBottomBaseItem>();
        UIWidget[] widgets = itemGo.GetComponentsInChildren<UIWidget>(true);
        for (int i = 0; i < widgets.Length; i++)
        {
            widgets[i].depth += baseDepth;
        }

        return item;
    }

    static public LocalBottomBaseItem GetTipBlockItem(GameObject parent, Vector3 localPos, int baseDepth)
    {
        return GetTipBlockItem(parent, localPos, baseDepth, false) as LocalBottomBaseItem;
    }

    static public LegacyBottomBaseItem GetTipSelectBlockItem(GameObject parent, Vector3 localPos, int baseDepth)
    {
        return GetTipBlockItem(parent, localPos, baseDepth, true);
    }

    /// <summary>
    /// 获得制定npc的半身像
    /// </summary>
    static public Texture GetNpcBustTex(int npcId)
    {
        NpcListTable nlt = TableManager.GetTableData((int)TableID.NpcListTableID, npcId) as NpcListTable;
        AvatarTable at = TableManager.GetTableData((int)TableID.AvatarTableID, nlt.avatarID) as AvatarTable;
        //MyLog.EditorLog("Textures/NPCBust/" + at.bustID + ".png");
        Texture tex = ResourceMgr.LoadAsset("Textures/NPCBust/" + at.bustID + ".png", at.bustID, typeof(Texture), true, true) as Texture;
        return tex;
    }

    /// <summary>
    /// 计算物体周围的一个随机点
    /// </summary>
    /// <param name="trans">以当前物体为中心的周围</param>
    /// <param name="minAngel">最小的角度，物体正前方为0，左为负，右为正</param>
    /// <param name="maxAngle">最大的角度，物体正前方为0，左为负，右为正</param>
    /// <param name="minDis">最小的距离</param>
    /// <param name="maxDis">最大的距离</param>
    static public Vector3 CaculateAroundRandomPos(Transform trans, float minAngel, float maxAngle, float minDis, float maxDis)
    {
        float rDis = UnityEngine.Random.Range(minDis, maxDis);
        float angle = UnityEngine.Random.Range(minAngel, maxAngle);
        float radians = Mathf.Deg2Rad * angle;
        Vector3 localPos = new Vector3();
        localPos.x = rDis * Mathf.Sin(radians);
        localPos.z = rDis * Mathf.Cos(radians);

        Vector3 worldPos = trans.TransformPoint(localPos);

        return worldPos;
    }

    /// <summary>
    /// 取得一个点半径周围内的一个点
    /// </summary>
    /// <param name="centerPos"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    static public Vector3 CaculateAroundRandomPos(Vector3 centerPos, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360);
        float radians = Mathf.Deg2Rad * angle;
        Vector3 localPos = new Vector3();
        localPos.x = radius * Mathf.Sin(radians);
        localPos.z = radius * Mathf.Cos(radians);

        Vector3 worldPos = centerPos + localPos;

        return worldPos;
    }

    /// <summary>
    /// 更改选中的目标
    /// </summary>
    static public void ChangeTarget(long pid)
    {
        CharacterBase characterBase = GameCoreController.Instance.GetCharacterByPID(pid, CharacterType.Any);
        if (characterBase != null)
        {
            LocalPlayer.Instance.LockEnemyByHand(characterBase);
        }
    }

    /// <summary>
    /// 检测玩家是否死亡状态并提示
    /// </summary>
    static public bool CheckDieTips()
    {
        if (LocalPlayer.Instance.IsDead || LocalPlayer.Instance.IsGhost)
        {
            WindowMgr.OpenInformWindow(InformType.FixNoticeType, StringConst.COMMON_DIE_LIMIT_TIPS);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 根据NPCID进行寻路操作
    /// </summary>
    static public void FindPathByNpcId(int npcId, PathDelegate arrival, bool openProgress, params object[] args)
    {
        NpcListTable npc = TableManager.GetTableData((int)TableID.NpcListTableID, npcId) as NpcListTable;
        if (npc == null)
        {
            MyLog.EditorLogError("Npc不存在    " + npcId);
            return;
        }
        SceneInfoForClient mapInfo = ResourceMgr.LoadMapInfo(npc.MapId) as SceneInfoForClient;
        NPCInfo info = mapInfo.Npcs.Find(p => p.NPCID == npcId);
        if (info == null)
        {
            MyLog.EditorLogError("地图 " + npc.MapId + " 中不存在Npc " + npcId + " " + npc.NpcName);
            return;
        }

        Transform tf = new GameObject().transform;
        tf.position = info.Position;
        tf.eulerAngles = info.Rotation;
        Vector3 funPos = GameUtils.CaculateAroundRandomPos(tf, -60, 60, 1, 1);
        MonoBehaviour.Destroy(tf.gameObject);

        if (openProgress)
        {
            LocalPlayer.Instance.AutoMoveToWithProgress(npc.MapId, funPos, arrival, args);
        }
        else
        {
            LocalPlayer.Instance.AutoMoveTo(npc.MapId, funPos, arrival, args);
        }
    }

    /// <summary>
    /// 取得玩家模型的资源ID
    /// </summary>
    static public int GetPlayerModelResId(AvatarTable avatar, int resId, EquipmentPart part, int value)
    {
        if (value > 10000) return value;

        int resModel = 0;
        int index = value;
        if (resId > 10000)
        {
            resId /= 100;
        }
        switch (part)
        {
            case EquipmentPart.Head:
                index = resId * 1000 + value;
                resModel = avatar.headModel;
                break;
            case EquipmentPart.Body:
                index = resId * 1000 + 100 + value;
                resModel = avatar.bodyModel;
                break;
            case EquipmentPart.Weapon:
            case EquipmentPart.SecWeapon:
                return value;
        }

        RoleModeTable table = TableManager.GetTableData((int)TableID.RoleModeTableID, index) as RoleModeTable;
        if (table != null)
        {
            resModel = table.resourcesId;

        }
        else if (value != 0)
        {
            MyLog.EditorLogError("RoleModelTable isn't exist:" + index);
        }

        return resModel;
    }

    /// <summary>
    /// 通过ItemID,获得到模型ID
    /// </summary>
    static public int GetModelIDByItemID(int itemID, EquipmentPart part)
    {
        EquipTable tb = TableManager.GetTableData((int)TableID.EquipTableID, itemID) as EquipTable;
        if (tb == null)
        {
            return 0;
        }

        switch (part)
        {
            case EquipmentPart.Head: return tb.head_model;
            case EquipmentPart.Body: return tb.body_model;
            case EquipmentPart.Weapon:
            case EquipmentPart.SecWeapon:
                return tb.item_model;
        }

        return 0;
    }

    /// <summary>
    /// 属性描述字符串分割
    /// </summary>
    static public string AttrDescSplit(string str, bool isLine)
    {
        StringBuilder stru = new StringBuilder();
        string[] strarr = str.Split(';');
        if (!string.IsNullOrEmpty(str) && strarr.Length == 0)
        {
            strarr[0] = str;
        }
        for (int i = 0; i < strarr.Length; i++)
        {
            if (strarr.Length >= 1)
            {
                string[] arr = strarr[i].Split(',');
                if (arr.Length > 1)
                {
                    stru.Append(GetItemAttrDesc((EquipAttrName)int.Parse(arr[0])));
                    stru.Append("+");
                    stru.Append(arr[1]);
                    stru = isLine ? stru.Append("\n") : stru.Append(" ");
                }
            }
        }
        return stru.ToString();
    }

    /// <summary>
    /// 服务器点与Unity坐标点的转换
    /// </summary>
    static public Vector3 GetServerPoint(Coord coord)
    {
        return new Vector3(coord.x, coord.z, coord.y);
    }

    #region 物品Item工具Func

    /// <summary>
    /// 获得物品大类描述
    /// </summary>
    static public string GetItemBaseTypeDesc(ItemType type)
    {
        string result = "";
        switch (type)
        {
            case ItemType.BAG_TYPE_ALL:
                result = StringConst.BAG_TYPE_ALL;
                break;
            case ItemType.BAG_TYPE_EQUIP:
                result = StringConst.BAG_TYPE_EQUIP;
                break;
            case ItemType.BAG_TYPE_CONSUMABLE:
                result = StringConst.BAG_TYPE_CONSUMABLE;
                break;
            case ItemType.BAG_TYPE_MATERIAL:
                result = StringConst.BAG_TYPE_MATERIAL;
                break;
            case ItemType.BAG_TYPE_CHIP:
                result = StringConst.BAG_TYPE_CHIP;
                break;
            case ItemType.BAG_TYPE_TREASURE:
                result = StringConst.BAG_TYPE_TREASURE;
                break;
        }
        return result;
    }

    /// <summary>
    /// 获得字段描述
    /// </summary>
    static public string GetItemAttrDesc(EquipAttrName att)
    {
        string result = "";
        switch (att)
        {
            case EquipAttrName.None:
                result = "基础属性：";
                break;
            case EquipAttrName.ARMOR:
                result = StringConst.ENTITY_ATTR_ARMOR;
                break;
            case EquipAttrName.ATTACK_SPEED:
                result = StringConst.ENTITY_ATTR_ATTACK_SPEED;
                break;
            case EquipAttrName.PHY_ATTACK_INTENSITY:
                result = StringConst.ENTITY_ATTR_PHY_ATTACK_INTENSITY;
                break;
            case EquipAttrName.MAGIC_ATTACK_INTENSITY:
                result = StringConst.ENTITY_ATTR_MAGIC_ATTACK_INTENSITY;
                break;
            case EquipAttrName.MAGIC_RESISTANCE:
                result = StringConst.ENTITY_ATTR_MAGIC_RESISTANCE;
                break;
            case EquipAttrName.HP:
                result = StringConst.ENTITY_ATTR_HP;
                break;
            case EquipAttrName.STRENGTH:
                result = StringConst.ENTITY_ATTR_STRENGTH;
                break;
            case EquipAttrName.AGILITY:
                result = StringConst.ENTITY_ATTR_AGILITY;
                break;
            case EquipAttrName.WIT:
                result = StringConst.ENTITY_ATTR_WIT;
                break;
            case EquipAttrName.STAMINA:
                result = StringConst.ENTITY_ATTR_STAMINA;
                break;
            case EquipAttrName.DODGE:
                result = StringConst.ENTITY_ATTR_DODGE;
                break;
            case EquipAttrName.WITHSTAND:
                result = StringConst.ENTITY_ATTR_WITHSTAND;
                break;
            case EquipAttrName.BLOCK:
                result = StringConst.ENTITY_ATTR_BLOCK;
                break;
            case EquipAttrName.CRIT:
                result = StringConst.ENTITY_ATTR_CRIT;
                break;
            case EquipAttrName.TENACITY:
                result = StringConst.ENTITY_ATTR_TENACITY;
                break;
            case EquipAttrName.LESSENCD_RATE:
                result = StringConst.ENTITY_ATTR_LESSENCD_RATE;
                break;
            case EquipAttrName.PROFICIENT:
                result = StringConst.ENTITY_ATTR_PROFICIENT;
                break;
            default: result = "";
                break;
        }

        return result;
    }

    /// <summary>
    /// 获得字段描述(区分是否是特殊属性)
    /// </summary>
    static public bool GetItemAttrDesc(EquipAttrName att, ref string result)
    {
        switch (att)
        {
            case EquipAttrName.None:
                result = StringConst.TIP_ATTRIBUTE_BASEATTR;
                return false;
            case EquipAttrName.ARMOR:
            case EquipAttrName.ATTACK_SPEED:
            case EquipAttrName.PHY_ATTACK_INTENSITY:
            case EquipAttrName.MAGIC_ATTACK_INTENSITY:
            case EquipAttrName.MAGIC_RESISTANCE:
            case EquipAttrName.HP:
            case EquipAttrName.STRENGTH:
            case EquipAttrName.AGILITY:
            case EquipAttrName.WIT:
            case EquipAttrName.STAMINA:
                result = GameUtils.GetItemAttrDesc(att);
                return false;
            case EquipAttrName.DODGE:
            case EquipAttrName.WITHSTAND:
            case EquipAttrName.BLOCK:
            case EquipAttrName.CRIT:
            case EquipAttrName.TENACITY:
            case EquipAttrName.LESSENCD_RATE:
            case EquipAttrName.PROFICIENT:
                result = GameUtils.GetItemAttrDesc(att);
                return true;
            default: result = "";
                return false;
        }
    }

    /// <summary>
    /// 获得装备类型
    /// </summary>
    static public string GetItemGoodTypeDesc(GoodsType type)
    {
        switch (type)
        {
            case GoodsType.GOODS_CLOTH:
                return StringConst.GOODS_CLOTH;
            case GoodsType.GOODS_LEATHER:
                return StringConst.GOODS_LEATHER;
            case GoodsType.GOODS_LOCKER:
                return StringConst.GOODS_LOCKER;
            case GoodsType.GOODS_BOARDER:
                return StringConst.GOODS_BOARDER;
            case GoodsType.GOODS_WEAPON:
                return StringConst.GOODS_WEAPON;
            case GoodsType.GOODS_ACCESSORY:
                return StringConst.GOODS_ACCESSORY;
            case GoodsType.GOODS_FASHION:
                return StringConst.GOODS_FASHION;
            case GoodsType.GOODS_PACKAGE:
                return StringConst.GOODS_PACKAGE;
            case GoodsType.GOODS_CONSUMABLE:
                return StringConst.GOODS_CONSUMABLE;
            case GoodsType.GOODS_JEWELRY:
                return StringConst.GOODS_JEWELRY;
            case GoodsType.GOODS_GLYPHS:
                return StringConst.GOODS_GLYPHS;
            case GoodsType.GOODS_TASKER:
                return StringConst.GOODS_TASKER;
            case GoodsType.GOODS_MATERIAL:
                return StringConst.GOODS_MATERIAL;
            case GoodsType.GOODS_GOOD:
                return StringConst.GOODS_GOOD;
            case GoodsType.GOODS_RECIPE:
                return StringConst.GOODS_RECIPE;
            case GoodsType.GOODS_KEY:
                return StringConst.GOODS_KEY;
            case GoodsType.GOODS_OTHER:
                return StringConst.GOODS_OTHER;
            case GoodsType.GOODS_MOUNT_CHIP:
                return StringConst.GOODS_MOUNT_CHIP;
            case GoodsType.GOODS_PET_CHIP:
                return StringConst.GOODS_PET_CHIP;
            case GoodsType.GOODS_SUITE_CHIP:
                return StringConst.GOODS_SUITE_CHIP;
            case GoodsType.GOODS_MOUNT_EXP:
                return StringConst.GOODS_MOUNT_EXP;
            case GoodsType.GOODS_PET_EXPITEM:
                return StringConst.GOODS_PET_EXPITEM;
            case GoodsType.GOODS_GOLD:
                return StringConst.GOODS_GOLD;
            case GoodsType.GOODS_DIAMOND:
                return StringConst.GOODS_DIAMOND;
            case GoodsType.GOODS_RUNES:
                return StringConst.GOODS_RUNES;
            case GoodsType.GOODS_SERVANT_CHIP:
                return StringConst.GOODS_SERVANT_CHIP;
            case GoodsType.GOODS_CHESTS:
                return StringConst.GOODS_CHESTS;
            case GoodsType.GOODS_SERVANT_TREASURE:
                return StringConst.GOODS_SERVANT_TREASURE;
            case GoodsType.GOODS_TREASURE_MAP:
                return StringConst.GOODS_TREASURE_MAP;
            case GoodsType.GOODS_MEDICINE:
                return StringConst.GOODS_MEDICINE;
            case GoodsType.GOODS_FOOD_BLOOD:
                return StringConst.GOODS_FOOD_BLOOD;
            case GoodsType.GOODS_FOOD_MAGIC:
                return StringConst.GOODS_FOOD_MAGIC;
            case GoodsType.GOODS_STATEMEDICINE:
                return StringConst.GOODS_STATEMEDICINE;
            case GoodsType.GOODS_HEIRLOOM:
                return StringConst.GOODS_HEIRLOOM;
            case GoodsType.GOODS_FRIEND:
                return StringConst.GOODS_FRIEND;
            case GoodsType.GOODS_CRYSTAL:
                return StringConst.GOODS_CRYSTAL;
            case GoodsType.GOODS_MONEYBUFF:
                return StringConst.GOODS_MONEYBUFF;
            case GoodsType.GOODS_EXPBUFF:
                return StringConst.GOODS_EXPBUFF;
            case GoodsType.GOODS_FXBUFF:
                return StringConst.GOODS_FXBUFF;
            case GoodsType.GOODS_BLOOD_PACKAGE:
                return StringConst.GOODS_BLOOD_PACKAGE;
            case GoodsType.GOODS_MAGIC_PACKAGE:
                return StringConst.GOODS_MAGIC_PACKAGE;
            case GoodsType.GOODS_BLOOD_INSTANT:
                return StringConst.GOODS_BLOOD_INSTANT;
            case GoodsType.GOODS_MAGIC_INSTANT:
                return StringConst.GOODS_MAGIC_INSTANT;
            case GoodsType.GOODS_BLOOD_CONTINUED:
                return StringConst.GOODS_BLOOD_CONTINUED;
            case GoodsType.GOODS_MAGIC_CONTINUED:
                return StringConst.GOODS_MAGIC_CONTINUED;
            case GoodsType.GOODS_STATEMEDICINE_SHORT:
                return StringConst.GOODS_STATEMEDICINE_SHORT;
            case GoodsType.GOODS_STATEMEDICINE_LONG:
                return StringConst.GOODS_STATEMEDICINE_LONG;
            case GoodsType.GOODS_SINGLE_SWORD:
                return StringConst.GOODS_SINGLE_SWORD;
            case GoodsType.GOODS_DRAGGER:
                return StringConst.GOODS_DRAGGER;
            case GoodsType.GOODS_SINGLE_MACE:
                return StringConst.GOODS_SINGLE_MACE;
            case GoodsType.GOODS_SINGLE_AXW:
                return StringConst.GOODS_SINGLE_AXW;
            case GoodsType.GOODS_STAFF:
                return StringConst.GOODS_STAFF;
            case GoodsType.GOODS_BOW:
                return StringConst.GOODS_BOW;
            case GoodsType.GOODS_GUN:
                return StringConst.GOODS_GUN;
            case GoodsType.GOODS_SHIELD:
                return StringConst.GOODS_SHIELD;
            case GoodsType.GOODS_BOTHHAND_SWORD:
                return StringConst.GOODS_BOTHHAND_SWORD;
            case GoodsType.GOODS_BOTHHAND_AXE:
                return StringConst.GOODS_BOTHHAND_AXE;
            case GoodsType.GOODS_BOTHHAND_MACE:
                return StringConst.GOODS_BOTHHAND_MACE;
            case GoodsType.GOODS_BOOK:
                return StringConst.GOODS_BOOK;
            case GoodsType.GOODS_LIGHT:
                return StringConst.GOODS_LIGHT;
            default: return "";
        }
    }

    /// <summary>
    /// 获得装备子类
    /// </summary>
    static public string GetItemSubTypeDesc(SubType type)
    {
        switch (type)
        {
            case SubType.NONE:
                return "";
            case SubType.BAG_HEAD:
                return StringConst.BAG_HEAD;
            case SubType.BAG_HAND:
                return StringConst.BAG_HAND;
            case SubType.BAG_BODY:
                return StringConst.BAG_BODY;
            case SubType.BAG_PANTS:
                return StringConst.BAG_PANTS;
            case SubType.BAG_GLOVE:
                return StringConst.BAG_GLOVE;
            case SubType.BAG_LEG:
                return StringConst.BAG_LEG;
            case SubType.BAG_DECORATION:
                return StringConst.BAG_DECORATION;
            case SubType.BAG_FASHION:
                return StringConst.BAG_FASHION;
            case SubType.BAG_BOTHHAND:
                return StringConst.BAG_BOTHHAND;
            case SubType.BAG_SINGLE:
                return StringConst.BAG_SINGLE;
            case SubType.BAG_DEPUTY:
                return StringConst.BAG_DEPUTY;
            default: return "";
        }
    }

    /// <summary>
    /// 获得阵营
    /// </summary>
    static public string GetCampStr(int campID)
    {
        if ((CampType)campID == CampType.Horde)
        {
            return StringConst.BFIELD_HORDE;
        }
        else if ((CampType)campID == CampType.Alliance)
        {
            return StringConst.BFIELD_ALLIA;
        }

        return "";
    }

    /// <summary>
    /// 获得金币数量
    /// </summary>
    static public int GetGoldNum(GameCoin type)
    {
        int result = 0;

        if (LocalPlayer.Instance != null)
        {
            switch (type)
            {
                case GameCoin.Gold: result = LocalPlayer.Instance.Attribute.Gold; break;
                case GameCoin.Diamond: result = LocalPlayer.Instance.Attribute.Jewel; break;
                case GameCoin.Crystal: result = LocalPlayer.Instance.Attribute.Crystal; break;
            }
        }

        return result;
    }

    /// <summary>
    /// 获得相应金币图标
    /// </summary>
    static public string GetGlodTypeName(int type)
    {
        string result = "icon_0002";
        switch ((GameCoin)type)
        {
            case GameCoin.Gold: result = "icon_0002"; break;
            case GameCoin.Diamond: result = "icon_0039"; break;
            case GameCoin.Honor: result = "icon_0037"; break;
            case GameCoin.GuildPersonalDonate: result = "icon_0036"; break;
            case GameCoin.ReputationValue: result = "icon_0002"; break;
            case GameCoin.GoodTeacherValue: result = "icon_0002"; break;
            case GameCoin.Crystal: result = "icon_shuijin"; break;
        }

        return result;
    }


    /// <summary>
    /// 填充文字
    /// </summary>
    static public void FillStrVal(ref string str1, ref string str2)
    {
        int length1 = str1.Length;
        int length2 = str2.Length;

        int max = Mathf.Abs(length1 - length2);
        if (length1 > length2)
        {
            for (int i = 0; i < max; i++)
            {
                str2 += " ";
            }
        }
        else
        {
            for (int i = 0; i < max; i++)
            {
                str1 += " ";
            }
        }
    }

    /// <summary>
    /// 获得金币称谓
    /// </summary>
    /// <returns></returns>
    static public string GetGoldName(int type)
    {
        string result = "";
        switch ((GameCoin)type)
        {
            case GameCoin.Gold: result = StringConst.GOODS_GOLD; break;
            case GameCoin.Diamond: result = StringConst.GOODS_DIAMOND; break;
            case GameCoin.Honor: result = StringConst.SHOP_TIP_HONORCOIN; break;
            case GameCoin.GuildPersonalDonate: result = StringConst.SHOP_TIP_GUILDCOIN; break;
            case GameCoin.ReputationValue: result = StringConst.SHOP_TIP_REPUTATIONCOIN; break;
            case GameCoin.GoodTeacherValue: result = StringConst.SHOP_TIP_APPRENTICECOIN; break;
            case GameCoin.Crystal: result = StringConst.GOODS_CRYSTAL; break;
        }

        return result;
    }

    /// <summary>
    /// 获得部位图标的图集
    /// </summary>
    static public string GetBodySpriteName(BodyPart part)
    {
        string str = "";

        switch (part)
        {
            case BodyPart.PLAYER_HEAD: str = "pic_222"; break;
            case BodyPart.PLAYER_HAND: str = "pic_217"; break;
            case BodyPart.PLAYER_BODY: str = "pic_216"; break;
            case BodyPart.PLAYER_GLOVE: str = "pic_218"; break;
            case BodyPart.PLAYER_PANTS: str = "pic_223"; break;
            case BodyPart.PLAYER_FOOT: str = "pic_219"; break;
            case BodyPart.PLAYER_MAINHAND: str = "pic_221"; break;
            case BodyPart.PLAYER_DEPUTY: str = "pic_220"; break;
        }

        return str;
    }

    /// <summary>
    /// 获得职业偏向类型的图标名称
    /// </summary>
    static public string GetJobTypeSpName(int job)
    {
        string result = "";

        switch (job)
        {
            case 1: result = "tanke-xiao"; break;
            case 2: result = "zhiliao-xiao"; break;
            case 3: result = "shanghaishuchu-wuli"; break;
            case 4: result = "shanghaishuchu-mofa"; break;
        }

        return result;
    }

    /// <summary>
    /// 通过对比数量获得相应格式的信息
    /// </summary>
    static public string NumCompare(int curNum, int needNum)
    {
        string str = "";
        if (curNum < needNum)
        {
            string color = ColorTo16(GameConst.UITextColors[UITextColor.NormalRed]);
            str = string.Format("[{0}]{1}[-]/{2}[-]", color, curNum, needNum);
            return str;
        }
        else
        {
            string color = ColorTo16(GameConst.UITextColors[UITextColor.NormalGreen]);
            str = string.Format("[{0}]{1}[-]/{2}[-]", color, curNum, needNum);
            return str;
        }
    }

    /// <summary>
    /// 获得玩家当前的专精
    /// </summary>
    static public int GetCurrentMastery()
    {
        if (LocalPlayer.Instance == null)
        {
            return 0;
        }

        int currentJob = LocalPlayer.Instance.Job;
        int currentLv = LocalPlayer.Instance.Attribute.Level;
        int currentFocus = currentJob * 100 + LocalPlayer.Instance.Attribute.Mastery;
        return currentFocus;
    }

    /// <summary>
    /// 获得当前玩家职业类所有专精，专精为0
    /// </summary>
    static public int GetCurrentComMast()
    {
        if (LocalPlayer.Instance == null)
        {
            return 0;
        }

        int currentJob = LocalPlayer.Instance.Job;
        int currentFocus = currentJob * 100;
        return currentFocus;
    }

    /// <summary>
    /// 通过道具类型获得相对应的装备槽
    /// </summary>
    static public int GetEquipSameIndex(SubType type)
    {
        int index = 0;

        switch (type)
        {
            case SubType.BAG_HEAD:
            case SubType.BAG_HAND:
            case SubType.BAG_BODY:
            case SubType.BAG_PANTS:
            case SubType.BAG_GLOVE:
            case SubType.BAG_LEG:
                index = (int)type - 1;
                break;
            case SubType.BAG_BOTHHAND:
            case SubType.BAG_SINGLE:
                index = 8;
                break;
            case SubType.BAG_DEPUTY:
                index = 9;
                break;
        }

        return index;
    }

    /// <summary>
    /// 获得打折的资源名称
    /// </summary>
    static public string GetDisCountName(int discount)
    {
        string str = "";
        switch (discount)
        {
            case 1: str = "icon_00190"; break;
            case 2: str = "icon_00191"; break;
            case 3: str = "icon_00192"; break;
            case 4: str = "icon_00193"; break;
            case 5: str = "icon_00194"; break;
            case 6: str = "icon_00195"; break;
            case 7: str = "icon_00196"; break;
            case 8: str = "icon_00197"; break;
            case 9: str = "icon_00198"; break;
            default: break;
        }

        return str;
    }

    /// <summary>
    /// 获取服务器状态的图标
    /// </summary>
    static public string GetServerState(ServerBase data)
    {
        string result = "";

        if (data == null)
        {
            return result;
        }

        if (data.IsMaintain)
        {
            result = "icon_00133";
        }
        else
        {
            switch (data.ServerState)
            {
                case 0: result = "icon_00131"; break;
                case 1: result = "icon_00130"; break;
            }
        }

        return result;
    }

    /// <summary>
    /// 获取服务器状态的图标
    /// </summary>
    static public string GetServerLineState(int state)
    {
        string result = "";
        switch (state)
        {
            case 0: result = "icon_00131"; break;
            case 1: result = "icon_00130"; break;
        }

        return result;
    }

    /// <summary>
    /// 获得装备品质颜色
    /// </summary>
    static public Color GetEquipQualityColor(int quality)
    {
        switch (quality)
        {
            case 0: return GameConst.UITextColors[UITextColor.NormalRed];
            case 1: return GameConst.UITextColors[UITextColor.Equip_Quality_White];
            case 2: return GameConst.UITextColors[UITextColor.Equip_Quality_Green];
            case 3: return GameConst.UITextColors[UITextColor.Equip_Quality_Blue];
            case 4: return GameConst.UITextColors[UITextColor.Equip_Quality_Purple];
            case 5: return GameConst.UITextColors[UITextColor.Equip_Quality_Orange];
            case 9: return GameConst.UITextColors[UITextColor.Equip_Quality_Heirloom];
            default:
                return GameConst.UITextColors[UITextColor.Equip_Quality_White];
        }
    }

    /// <summary>
    /// 获得品质名称
    /// </summary>
    static public string GetQualityChinese(int quality)
    {
        switch (quality)
        {
            case 1: return StringConst.EQUIP_QUALITY_TYPE1;
            case 2: return StringConst.EQUIP_QUALITY_TYPE2;
            case 3: return StringConst.EQUIP_QUALITY_TYPE3;
            case 4: return StringConst.EQUIP_QUALITY_TYPE4;
            case 5: return StringConst.EQUIP_QUALITY_TYPE5;
            default:
                return StringConst.EQUIP_QUALITY_TYPE1;
        }
    }

    /// <summary>
    /// 获得带有颜色BBC的品质字符串
    /// </summary>
    static public string GetQualityWithColor(int quality)
    {
        string result = "";
        string color = ColorTo16(GetEquipQualityColor(quality));
        result = string.Format("[{0}]{1}[-]", color, GetQualityChinese(quality));
        return result;
    }

    /// <summary>
    /// 获得该品质下的字体
    /// </summary>
    static public string GetStrWithColor(string value, int quality)
    {
        string result = "";
        string color = ColorTo16(GetEquipQualityColor(quality));
        result = string.Format("[{0}]{1}[-]", color, value);
        return result;
    }

    /// <summary>
    /// 返回颜色的BBCode
    /// </summary>
    static public string GetColorBBCode(string value, UITextColor color)
    {
        string colorStr = ColorTo16(GameConst.UITextColors[color]);
        return string.Format("[{0}]{1}[-]", colorStr, value);
    }

    /// <summary>
    /// 获得物品的详细描述
    /// </summary>
    static public string GetItemInfo(ItemBase itemData)
    {
        StringBuilder buidler = new StringBuilder();

        if (itemData == null || itemData.ItemTB == null)
        {
            return "";
        }

        //1.物品描述
        if (itemData.GoodType == GoodsType.GOODS_CHESTS)
        {
            ResBoxInfo temp = null;
            if (ItemMgr.Instance.BoxInfo.TryGetValue(itemData.ItemId, out temp))
            {
                if (temp != null)
                {
                    if (!temp.desc.Contains("{0}"))
                    {
                        //不存在占位符
                        buidler.Append(temp.desc.Replace("\\n", "\n"));
                        buidler.Append("\n");
                    }
                    else
                    {
                        //存在占位符,进行筛选
                        StringBuilder builder = new StringBuilder();

                        if (temp.box_type == 6)
                        {
                            //专精类可选择宝箱
                            int currentFocus = GameUtils.GetCurrentMastery();
                            int currentTwo = GameUtils.GetCurrentComMast();
                            int length = temp.award_info.Count;
                            for (int i = 0; i < length; i++)
                            {
                                if (temp.award_info[i].item_mastery != currentFocus && temp.award_info[i].item_mastery != currentTwo)
                                {
                                    continue;
                                }

                                ItemTable item = TableManager.GetTableData((int)TableID.ItemTableID, temp.award_info[i].item_id) as ItemTable;
                                if (item == null)
                                {
                                    continue;
                                }

                                builder.Append(GameUtils.GetStrWithColor(item.item_name, item.item_quality));
                                builder.Append("×");
                                builder.Append(temp.award_info[i].item_count);
                                builder.Append("\n");
                            }

                            string str = builder.ToString();
                            if (str.Length > 0)
                            {
                                str = str.Substring(0, str.Length - 1);
                            }

                            buidler.Append(string.Format(temp.desc, str));
                            buidler.Append("\n");
                        }
                        else
                        {
                            //一般可选择宝箱
                            int length = temp.award_info.Count;
                            for (int i = 0; i < length; i++)
                            {
                                ItemTable item = TableManager.GetTableData((int)TableID.ItemTableID, temp.award_info[i].item_id) as ItemTable;
                                if (item == null)
                                {
                                    continue;
                                }

                                builder.Append(GameUtils.GetStrWithColor(item.item_name, item.item_quality));

                                if (temp.box_type != 2 && temp.box_type != 5 && temp.box_type != 7)
                                {
                                    builder.Append("×");
                                    builder.Append(temp.award_info[i].item_count);
                                }
                                builder.Append("\n");
                            }

                            string str = builder.ToString();
                            if (str.Length > 0)
                            {
                                str = str.Substring(0, str.Length - 1);
                            }
                            buidler.Append(string.Format(temp.desc, str));
                            buidler.Append("\n");
                        }
                    }
                }
            }
            else
            {
                MyLog.EditorLogError("No Box Cached Data,id=" + itemData.ItemId);
                BagDataMgr.Instance.ReqBoxInfo(itemData.ItemId);
            }
        }
        else if (!"0".Equals(itemData.ItemTB.item_des) && string.IsNullOrEmpty(itemData.ItemTB.item_des))
        {
            buidler.Append(itemData.ItemTB.item_des);
            buidler.Append("\n");
        }

        //2.物品等级
        /*
        if (itemData.ItemTB.item_level != 0)
        {
            buidler.Append(string.Format(StringConst.BAG_TIP_OBJLEVEL, itemData.ItemTB.item_level));
            buidler.Append("\n");
        }*/

        //3.物品使用等级
        if (itemData.ItemTB.use_level != 0)
        {
            buidler.Append(string.Format(StringConst.BAG_TIP_NEEDLEVEL, itemData.ItemTB.use_level));
            buidler.Append("\n");
        }

        //4.物品所属部位、专精、职业、阵营
        if (itemData.BaseType == ItemType.BAG_TYPE_EQUIP)
        {
            //装备部位
            if (itemData.GoodSubType != SubType.NONE)
            {
                buidler.Append(StringConst.BAG_TIP_EQUIPSED_BODYPART);
                buidler.Append(GameUtils.GetItemSubTypeDesc(itemData.GoodSubType));
                buidler.Append("\n");
            }

            //适用职业
            if (!GameUtils.JudgeIsWeapon(itemData.GoodType))
            {
                string[] jobs = itemData.ItemTB.occupation.Trim().Split(';');
                if (!"0".Equals(itemData.ItemTB.occupation.Trim()) && jobs.Length > 0)
                {
                    buidler.Append(StringConst.BAG_TIP_EQUIPSED_JOB);
                    for (int i = 0; i < jobs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(jobs[i]))
                        {
                            int tt = int.Parse(jobs[i].Trim());
                            buidler.Append(GameConst.JobName[tt]);
                        }

                        if (i < jobs.Length - 1)
                        {
                            buidler.Append("、");
                        }
                    }
                    buidler.Append("\n");
                }
            }

            //适用专精
            List<string> matery = new List<string>(itemData.ItemTB.specialization.Trim().Split(';'));
            if (!"0".Equals(itemData.ItemTB.specialization.Trim()) && matery.Count > 0)
            {
                buidler.Append(StringConst.BAG_TIP_EQUIPSED_MASTERY);
                for (int i = 0; i < matery.Count; i++)
                {
                    if (!string.IsNullOrEmpty(matery[i]))
                    {
                        int tt = int.Parse(matery[i].Trim());
                        buidler.Append(GameConst.MasteryName[tt]);
                    }

                    if (i < matery.Count - 1)
                    {
                        buidler.Append("、");
                    }
                }

                buidler.Append("\n");
            }
        }
        else
        {
            if (itemData.BaseType == ItemType.BAG_TYPE_CONSUMABLE && itemData.GoodType == GoodsType.GOODS_FASHION)
            {
                //时装适用职业
                if (!GameUtils.JudgeIsWeapon(itemData.GoodType))
                {
                    string[] jobs = itemData.ItemTB.occupation.Trim().Split(';');
                    if (!"0".Equals(itemData.ItemTB.occupation.Trim()) && jobs.Length > 0)
                    {
                        buidler.Append(StringConst.BAG_TIP_EQUIPSED_JOB);
                        for (int i = 0; i < jobs.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(jobs[i]))
                            {
                                int tt = int.Parse(jobs[i].Trim());
                                buidler.Append(GameConst.JobName[tt]);
                            }

                            if (i < jobs.Length - 1)
                            {
                                buidler.Append("、");
                            }
                        }
                        buidler.Append("\n");
                    }
                }

                //时装适用阵营
                if (itemData.ItemTB.camp_limit != 0)
                {
                    buidler.Append(StringConst.BAG_TIP_CAMP_LIMIT);
                    buidler.Append(GameUtils.GetCampStr(itemData.ItemTB.camp_limit));
                    buidler.Append("\n");
                }

                //时装适用专精
                List<string> matery = new List<string>(itemData.ItemTB.specialization.Trim().Split(';'));
                if (!"0".Equals(itemData.ItemTB.specialization.Trim()) && matery.Count > 0)
                {
                    buidler.Append(StringConst.BAG_TIP_EQUIPSED_MASTERY);
                    for (int i = 0; i < matery.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(matery[i]))
                        {
                            int tt = int.Parse(matery[i].Trim());
                            buidler.Append(GameConst.MasteryName[tt]);
                        }

                        if (i < matery.Count - 1)
                        {
                            buidler.Append("、");
                        }
                    }

                    buidler.Append("\n");
                }
            }

            //5.物品类型
            if (itemData.GoodType != GoodsType.NONE)
            {
                buidler.Append(StringConst.BAG_TIP_ITEMTYPE);
                buidler.Append(GameUtils.GetItemGoodTypeDesc(itemData.GoodType));
                buidler.Append("\n");
            }

            //6.物品用途
            if (!"0".Equals(itemData.ItemTB.purpose))
            {
                buidler.Append(StringConst.BAG_TIP_ITEMUSE);
                buidler.Append(itemData.ItemTB.purpose);
            }
        }

        return buidler.ToString();
    }

    /// <summary>
    /// 添加基础属性框
    /// </summary>
    static public string AddBaseAttr(int value, EquipAttrName att, bool fit = true)
    {
        StringBuilder result = new StringBuilder();
        string tempStr = "";
        bool isSpecial = GameUtils.GetItemAttrDesc(att, ref tempStr);
        tempStr = string.Format("{0}: +{1}", tempStr, value);

        if (fit)
        {
            result.Append(isSpecial ? GameUtils.GetColorBBCode(tempStr, UITextColor.NormalGreen) : tempStr);
        }
        else
        {
            result.Append(GameUtils.GetColorBBCode(tempStr, UITextColor.NormalGray));
        }

        return result.ToString();
    }

    /// <summary>
    /// 获得装备详细属性
    /// </summary>
    static public string GetEquipAttr(ItemBase data)
    {
        StringBuilder tempStr = new StringBuilder();

        if (data == null || data.ItemTB == null || !(data is EquipmentItemData))
        {
            return tempStr.ToString();
        }

        EquipmentItemData tempData = data as EquipmentItemData;

        Dictionary<EquipAttrName, int> fit = new Dictionary<EquipAttrName, int>();
        Dictionary<EquipAttrName, int> notFit = new Dictionary<EquipAttrName, int>();

        foreach (var pair in tempData.EquipAttrDic)
        {
            if (GameUtils.JudgeAttrFitPlayer((int)pair.Key))
            {
                fit.Add(pair.Key, pair.Value);
            }
            else
            {
                notFit.Add(pair.Key, pair.Value);
            }
        }

        foreach (var pair in fit)
        {
            tempStr.Append(AddBaseAttr(pair.Value, pair.Key, true));
            tempStr.Append("\n");
        }

        foreach (var pair in notFit)
        {
            tempStr.Append(AddBaseAttr(pair.Value, pair.Key, false));
            tempStr.Append("\n");
        }

        return tempStr.ToString();
    }

    /// <summary>
    /// 通过DripTable中的ID获得掉落包中的物品
    /// </summary>
    static public DropPackageTable GetDropData(int dropID)
    {
        DropTable tb = TableManager.GetTableData((int)TableID.DropTableID, dropID) as DropTable;
        if (tb == null || tb.drop_package.Length == 0)
        {
            return null;
        }

        DropPackageTable package = TableManager.GetTableData((int)TableID.DropPackageTableID, tb.drop_package[0].drop_id * 10) as DropPackageTable;

        return package;
    }

    /// <summary>
    /// 获得VIP称号名称
    /// </summary>
    static public string GetVIPName(int VIP)
    {
        string result = "";
        VipInfoTable tb = TableManager.GetTableData((int)TableID.VipInfoTableID, VIP) as VipInfoTable;
        if (tb != null)
        {
            result = tb.name;
        }

        return result;
    }

    /// <summary>
    /// 获取vip等级下的图标名称
    /// </summary>
    /// <param name="lv">等级</param>
    /// <param name="type">类型，1为四个字的，2位两个字的</param>
    /// <returns></returns>
    static public string GetVIPIconName(int lv, int type)
    {
        string result = "";
        VipInfoTable tb = TableManager.GetTableData((int)TableID.VipInfoTableID, lv) as VipInfoTable;
        if (tb == null)
        {
            MyLog.EditorLogError("vip数据表有误  " + lv);
        }
        else
        {
            result = tb.icon;
            if (type == 2)
            {
                result += "2";
            }
        }

        return result;
    }

    /// <summary>
    /// 判断VIP等级是否符合
    /// </summary>
    static public bool JudgeVIPEnough(VIP_POWER_TYPE vip)
    {
        bool result = false;

        VipInfoTable tb = TableManager.GetTableData((int)TableID.VipInfoTableID, LocalPlayer.Instance.Attribute.VipLevel) as VipInfoTable;
        if (tb != null)
        {
            string[] content = tb.power_list.Replace(" ", "").Split(',');
            for (int i = 0; i < content.Length; i++)
            {
                int id = int.Parse(content[i]);
                VipPowerTable st = TableManager.GetTableData((int)TableID.VipPowerTableID, id) as VipPowerTable;
                if (st != null && st.type == (int)vip)
                {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 判断装备上是否有宝石
    /// </summary>
    static public bool JudgeHasGem(ItemBase data)
    {
        bool result = false;
        if (data == null || data.ItemTB == null || data.ItemBsUpdate == null)
        {
            return result;
        }

        if (data.ItemBsUpdate.holeList.Count > 0)
        {
            for (int i = 0; i < data.ItemBsUpdate.holeList.Count; i++)
            {
                if (data.ItemBsUpdate.holeList[i].gemId > 0)
                {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 判断属性是否符合玩家
    /// </summary>
    static public bool JudgeAttrFitPlayer(int attrID)
    {
        bool result = false;
        int player = GetCurrentMastery();

        string arr = string.Format("{0}", attrID);
        SpecializationTable speTb = TableManager.GetTableData((int)TableID.SpecializationTableID, player) as SpecializationTable;
        if (speTb == null)
        {
            return result;
        }

        string[] attrs = speTb.effect_attr.Split(';');

        for (int i = 0; i < attrs.Length; i++)
        {
            if (attrs[i] == arr)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 判断玩家当前专精是否符合
    /// </summary>
    static public bool JudgeSpecialization(int special)
    {
        if (LocalPlayer.Instance == null)
        {
            return false;
        }

        int currentJob = LocalPlayer.Instance.Job;
        int currentLv = LocalPlayer.Instance.Attribute.Level;
        int currentFocus = currentJob * 100 + LocalPlayer.Instance.Attribute.Mastery;

        return currentFocus == special;
    }

    /// <summary>
    /// 判断是否是武器
    /// </summary>
    static public bool JudgeIsWeapon(GoodsType type)
    {
        if ((int)type > 500 || (type) == GoodsType.GOODS_WEAPON)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 判断是否是装备
    /// </summary>
    static public bool JudgeIsEquip(ItemBase data)
    {
        bool result = false;

        if (data == null || data.ItemTB == null)
        {
            return result;
        }

        result = data.GoodSubType != SubType.NONE;

        return result;
    }

    /// <summary>
    /// 判断当前物品是否适合玩家
    /// </summary>
    static public bool JudgeIsFitPlayer(int itemID)
    {
        if (LocalPlayer.Instance == null)
        {
            return false;
        }

        ItemTable tempTb = TableManager.GetTableData((int)TableID.ItemTableID, itemID) as ItemTable;
        if (tempTb == null)
        {
            MyLog.EditorLogError(string.Format("当前ItemID:{0}不存在！", itemID));
            return false;
        }

        //1.判断使用等级
        int level = tempTb.use_level;

        if (LocalPlayer.Instance.Attribute.Level < level)
        {
            return false;
        }

        //2.判断是否符合自己的职业或者专精
        if ((ItemType)tempTb.type == ItemType.BAG_TYPE_EQUIP)
        {
            if (!GameUtils.JudgeIsWeapon((GoodsType)tempTb.item_type))
            {
                //如果是武器就进行专精判断，否则进行职业判断
                string[] jobs = tempTb.occupation.Trim().Split(';');
                if (!"0".Equals(tempTb.occupation.Trim()) && jobs.Length > 0)
                {
                    bool hasSelfJob = false;
                    for (int i = 0; i < jobs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(jobs[i]))
                        {
                            int tt = int.Parse(jobs[i].Trim());
                            if (LocalPlayer.Instance.Attribute.Job == tt)
                            {
                                hasSelfJob = true;
                                break;
                            }
                        }
                    }

                    if (!hasSelfJob)
                    {
                        return false;
                    }
                }
            }
            else
            {
                List<string> matery = new List<string>(tempTb.specialization.Trim().Split(';'));
                if (!"0".Equals(tempTb.specialization.Trim()) && matery.Count > 0)
                {
                    //当前专精
                    int currentMastery = GameUtils.GetCurrentMastery();

                    bool hasSelfMastery = false;
                    for (int i = 0; i < matery.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(matery[i]))
                        {
                            int tt = int.Parse(matery[i].Trim());
                            if (currentMastery == tt)
                            {
                                hasSelfMastery = true;
                                break;
                            }
                        }
                    }

                    if (!hasSelfMastery)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 判断当前物品是否符合职业
    /// </summary>
    static public bool JudgeIsFitJob(ItemBase item)
    {
        if (item == null || item.ItemTB == null || LocalPlayer.Instance == null)
        {
            return false;
        }

        if ("0".Equals(item.ItemTB.occupation.Trim()))
        {
            return true;
        }

        //进行职业判断
        bool result = false;
        string[] jobs = item.ItemTB.occupation.Trim().Split(';');
        if (jobs.Length > 0)
        {
            for (int i = 0; i < jobs.Length; i++)
            {
                if (!string.IsNullOrEmpty(jobs[i]))
                {
                    int tt = int.Parse(jobs[i].Trim());
                    if (LocalPlayer.Instance.Attribute.Job == tt)
                    {
                        result = true;
                        break;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 判断当前物品是否符合专精
    /// </summary>
    static public bool JudgeIsFitSpecilization(ItemBase tb)
    {
        if (tb == null || tb.ItemTB == null)
        {
            return false;
        }

        if ("0".Equals(tb.ItemTB.specialization.Trim()))
        {
            return true;
        }

        //进行职业判断
        bool result = false;
        List<string> matery = new List<string>(tb.ItemTB.specialization.Trim().Split(';'));
        if (matery.Count > 0)
        {
            //当前专精
            int currentMastery = GameUtils.GetCurrentMastery();

            for (int i = 0; i < matery.Count; i++)
            {
                if (!string.IsNullOrEmpty(matery[i]))
                {
                    int tt = int.Parse(matery[i].Trim());
                    if (currentMastery == tt)
                    {
                        result = true;
                        break;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 判断当前专精是否可以用两把一样的武器
    /// </summary>
    static public bool JudgeCanUseSameWeapon()
    {
        int currentMastery = GameUtils.GetCurrentMastery();
        SpecializationTable tb = TableManager.GetTableData((int)TableID.SpecializationTableID, currentMastery) as SpecializationTable;

        if (tb == null)
        {
            return false;
        }
        else
        {
            return tb.main_hand == tb.weapon_hand;
        }
    }

    /// <summary>
    /// 判断主副手武器是否符合专精
    /// </summary>
    static public bool JudgeSpecialization(ItemBase item)
    {
        if (item == null || item.ItemTB == null)
        {
            return false;
        }

        int currentFocus = GameUtils.GetCurrentMastery();

        //2.专精符合
        SpecializationTable tb = TableManager.GetTableData((int)TableID.SpecializationTableID, currentFocus) as SpecializationTable;

        if (item.GoodSubType == (SubType)tb.main_hand || item.GoodSubType == (SubType)tb.weapon_hand)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 判断一件装备是否符合玩家穿戴需求
    /// </summary>
    static public bool JudegEquipFitPlayer(ItemBase item, bool compareLevel = false)
    {
        bool result = false;

        if (LocalPlayer.Instance == null || item == null || item.ItemTB == null || item.BaseType != ItemType.BAG_TYPE_EQUIP)
        {
            return false;
        }

        if (compareLevel)
        {
            //如果需要进行等级比对
            int level = item.ItemTB.use_level;

            if (LocalPlayer.Instance.Attribute.Level < level)
            {
                return false;
            }
        }

        if (!GameUtils.JudgeIsWeapon(item.GoodType))
        {
            //是装备直接比对职业
            if (GameUtils.JudgeIsFitJob(item))
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            //是武器直接比对专精
            if (GameUtils.JudgeIsFitSpecilization(item))
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }

        return result;
    }

    /// <summary>
    /// 判断一件装备是否是身上穿的
    /// </summary>
    static public bool JudgeEquipIsSuit(ItemBase data)
    {
        bool result = false;

        if (data == null || data.ItemTB == null || data.ItemBsUpdate == null)
        {
            return result;
        }

        List<ItemBase> list = BagDataMgr.Instance.GetCurrentSuitList();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].UniqueMark == data.UniqueMark)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 计算装备战斗力
    /// </summary>
    static public int CalculateEquipPower(ItemBase data, bool hasGem = true)
    {
        float result = 0;

        if (data == null || data.ItemTB == null || data.ItemBsUpdate == null)
        {
            return (int)result;
        }

        EquipmentItemData tempData = data as EquipmentItemData;

        //1.计算基础属性产生的战斗力
        foreach (var pair in tempData.EquipAttrDic)
        {
            if (!JudgeAttrFitPlayer((int)pair.Key))
            {
                continue;
            }

            CoefficientTable tb = TableManager.GetTableData((int)TableID.CoefficientTableID, (int)pair.Key) as CoefficientTable;

            if (tb != null)
            {
                result += pair.Value * tb.propvalue / 10000f;
            }
        }

        //2.计算附魔产生的战斗力
        EquipEnchantTable ttb = TableManager.GetTableData((int)TableID.EquipEnchantTableID, tempData.ItemBsUpdate.enchantid) as EquipEnchantTable;
        if (ttb != null)
        {
            result += ttb.nFightPower;
        }

        //3.计算宝石产生的战斗力
        List<ProtoHole> holes = tempData.ItemBsUpdate.holeList;
        for (int i = 0; i < holes.Count; i++)
        {
            if (holes[i].gemId != 0)
            {
                GemTable tb = TableManager.GetTableData((int)TableID.GemTableID, holes[i].gemId) as GemTable;
                if (tb != null)
                {
                    result += tb.nFightPower;
                }
            }
        }

        //4.计算技能产生的战斗力
        if (hasGem)
        {
            List<int> charAttrTb = tempData.ItemBsUpdate.skilllist;
            for (int i = 0; i < charAttrTb.Count; i++)
            {
                EffectTriggerTable effect = TableManager.GetTableData((int)TableID.EffectTriggerTableID, charAttrTb[i]) as EffectTriggerTable;
                if (effect != null)
                {
                    result += effect.nFightPower;
                }
            }
        }

        return Mathf.RoundToInt(result);
    }

    /// <summary>
    /// 检测是否有空包裹
    /// </summary>
    static public bool CheckNullPackage()
    {
        bool result = false;
        Dictionary<int, ItemBase> packageItems = BagDataMgr.Instance.GetItemsData.PackageItems;

        if (packageItems.Count <= 0)
        {
            return result;
        }

        for (int i = 0; i < 6; i++)
        {
            if (!packageItems.ContainsKey(i))
            {
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 检测物品是否符合使用等级
    /// </summary>
    /// <returns></returns>
    static public bool CheckItemNeedLevel(ItemBase data)
    {
        bool result = false;
        if (LocalPlayer.Instance == null || data == null || data.ItemTB == null)
        {
            return result;
        }

        if (LocalPlayer.Instance.Attribute.Level >= data.ItemTB.use_level)
        {
            result = true;
        }

        return result;
    }

    #endregion

    /// <summary>
    /// Gets the physical address.
    /// </summary>
    /// <returns>The physical address.</returns>
    static public string GetPhysicalAddress()
    {
        //NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();

        //foreach (NetworkInterface ni in nis)
        //{
        //    string addr = ni.GetPhysicalAddress().ToString();

        //    MyLog.EditorLog("mac = " + ni.GetPhysicalAddress().ToString());
        //    if (!string.IsNullOrEmpty(addr))
        //        return addr;
        //}

        return "null";
    }

    /// <summary>
    /// Gets the rand signature.
    /// </summary>
    /// <returns>The rand signature.</returns>
    static public string GetRandSignature()
    {
        string sign = PlayerPrefs.GetString("RegSignature");

        if (string.IsNullOrEmpty(sign))
        {
            sign = UnityEngine.Random.value.ToString();
            PlayerPrefs.SetString("RegSignature", sign);
        }
        //MyLog.EditorLog("sign="+sign);
        return sign;
    }

    /// <summary>
    /// 自动寻路到指定地图的出生点
    /// </summary>
    static public void AutoMoveToMapBornPos(int mapId, PathDelegate arrival, params object[] args)
    {
        if (SceneMgr.Instance.SceneInfo.MapID != mapId)
        {
            SceneInfoForClient mapInfo = ResourceMgr.LoadMapInfo(mapId) as SceneInfoForClient;
            Vector3 mapBornPos = mapInfo.Borns.Find(p => p.Camp == (CampType)LocalPlayer.Instance.Attribute.CampID).Pos;
            LocalPlayer.Instance.AutoMoveTo(mapId, mapBornPos, arrival, args);
        }
    }

    /// <summary>
    /// 从当前地图中找一个合适的传送门
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    static public PortalInfo GetCurrMapPortal(int mapId)
    {
        for (int i = 0; i < SceneMgr.Instance.SceneInfo.MapInfo.Portals.Count; i++)
        {
            PortalInfo port = SceneMgr.Instance.SceneInfo.MapInfo.Portals[i];
            ScenePortalTable tab = TableManager.GetTableData((int)TableID.ScenePortalTableID, port.ID) as ScenePortalTable;

            if (tab == null)
            {
                MyLog.EditorLogWarning("ScenePortalTable is null, id = " + port.ID);
                continue;
            }
            if (tab.nMapId == mapId)
            {
                return port;
            }
        }
        for (int i = 0; i < SceneMgr.Instance.SceneInfo.MapInfo.Portals.Count; i++)
        {
            PortalInfo port = SceneMgr.Instance.SceneInfo.MapInfo.Portals[i];
            ScenePortalTable tab = TableManager.GetTableData((int)TableID.ScenePortalTableID, port.ID) as ScenePortalTable;

            if (tab == null)
            {
                MyLog.EditorLogWarning("ScenePortalTable is null, id = " + port.ID);
                continue;
            }
            if (tab.navigate == LocalPlayer.Instance.Attribute.CampID)
                return port;
        }
        return null;
    }

    static public void SetCamera(bool Login)
    {
        if (Camera.main == null)
        {
            return;
        }
        if (Login)
        {
            Camera.main.orthographic = true;//正交
            Camera.main.orthographicSize = 5;
            Camera.main.nearClipPlane = 0.3f;
            Camera.main.farClipPlane = 1000f;
        }
        else
        {
            Camera.main.orthographic = false;//正交
            Camera.main.fieldOfView = 47;
            Camera.main.nearClipPlane = 0.3f;
            Camera.main.farClipPlane = 1000f;
        }
    }

    /// <summary>
    /// 获得战斗力美术字体
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    static public string GetPowerNumberString(int number, PowerWord.PowerWordType type)
    {
        string numStr = number.ToString();
        StringBuilder str = new StringBuilder();
        int value;
        for (int i = 0; i < numStr.Length; i++)
        {
            value = 0;
            value += ((int)type + numStr[i]) - 48;
            str.Append(value);
        }
        return str.ToString();
    }

    /// <summary>
    /// 获得物品品质框名称
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    static public string GetItemQualityBG(int quality)
    {
        return "itembg_00" + quality;
    }

    static public int ClampIndex(int val, int min, int max)
    {
        if (val < min)
        {
            val = min;
        }

        if (val > max)
        {
            val = max;
        }

        return val;
    }

    static public int ClampIndex(int val, int min)
    {
        if (val < min)
        {
            val = min;
        }

        return val;
    }

    /// <summary>
    /// 获得专精下的战场定位图标
    /// </summary>
    /// <param name="job"></param>
    /// <returns></returns>
    public static string GetSpecPosSpriteName(int spec)
    {
        //SpecializationTable st = TableManager.GetTableData((int)TableID.SpecializationTableID, job) as SpecializationTable;
        if (spec == 4)
        {
            spec = 3;
        }
        return GameConst.BattlePos32[spec];
    }

    /// <summary>
    /// 根据稀有度获取稀有度Icon SpriteName
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public static string GetRarityInconName(int rarity)
    {
        if (rarity < 1 || rarity > 5)
            return "";
        return "icon_0020" + (rarity - 1);
    }
}
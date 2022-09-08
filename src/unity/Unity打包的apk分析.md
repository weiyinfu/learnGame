# 目录
```plain
.
├── AndroidManifest.xml
├── apktool.yml
├── assets
│   ├── bin
│   │   └── Data
│   │       ├── Managed
│   │       │   ├── Metadata
│   │       │   │   └── global-metadata.dat
│   │       │   ├── Resources
│   │       │   │   └── mscorlib.dll-resources.dat
│   │       │   └── etc
│   │       │       └── mono
│   │       │           ├── 2.0
│   │       │           │   ├── Browsers
│   │       │           │   │   └── Compat.browser
│   │       │           │   ├── DefaultWsdlHelpGenerator.aspx
│   │       │           │   ├── machine.config
│   │       │           │   ├── settings.map
│   │       │           │   └── web.config
│   │       │           ├── 4.0
│   │       │           │   ├── Browsers
│   │       │           │   │   └── Compat.browser
│   │       │           │   ├── DefaultWsdlHelpGenerator.aspx
│   │       │           │   ├── machine.config
│   │       │           │   ├── settings.map
│   │       │           │   └── web.config
│   │       │           ├── 4.5
│   │       │           │   ├── Browsers
│   │       │           │   │   └── Compat.browser
│   │       │           │   ├── DefaultWsdlHelpGenerator.aspx
│   │       │           │   ├── machine.config
│   │       │           │   ├── settings.map
│   │       │           │   └── web.config
│   │       │           ├── browscap.ini
│   │       │           ├── config
│   │       │           └── mconfig
│   │       │               └── config.xml
│   │       ├── UnitySubsystems
│   │       │   └── PxrPlatform
│   │       │       └── UnitySubsystemsManifest.json
│   │       ├── boot.config
│   │       ├── data.unity3d
│   │       ├── sharedassets0.resource
│   │       └── unity\ default\ resources
│   └── res.json
├── lib
│   └── arm64-v8a
│       ├── libPicoAmbisonicDecoder.so
│       ├── libPicoSpatializer.so
│       ├── libPxrPlatform.so
│       ├── libil2cpp.so
│       ├── libmain.so
│       ├── libpxr_api.so
│       ├── libpxrplatformloader.so
│       └── libunity.so
├── original
│   ├── AndroidManifest.xml
│   └── META-INF
│       ├── CERT.RSA
│       ├── CERT.SF
│       └── MANIFEST.MF
├── res
│   ├── mipmap-anydpi
│   │   ├── app_icon.xml
│   │   └── app_icon_round.xml
│   ├── mipmap-mdpi
│   │   ├── app_icon.png
│   │   ├── ic_launcher_background.png
│   │   └── ic_launcher_foreground.png
│   ├── values
│   │   ├── ids.xml
│   │   ├── public.xml
│   │   ├── strings.xml
│   │   └── styles.xml
│   └── values-v30
│       └── strings.xml
└── smali
    ├── bitter
    │   └── jnibridge
    │       ├── JNIBridge$a.smali
    │       └── JNIBridge.smali
    ├── com
    │   ├── bytedance
    │   │   └── learPicoXr
    │   │       ├── BuildConfig.smali
    │   │       ├── R$id.smali
    │   │       ├── R$mipmap.smali
    │   │       ├── R$string.smali
    │   │       ├── R$style.smali
    │   │       └── R.smali
    │   ├── example
    │   │   └── tobserviceclient
    │   │       └── MainActivity.smali
    │   ├── google
    │   │   ├── androidgamesdk
    │   │   │   ├── ChoreographerCallback$1.smali
    │   │   │   ├── ChoreographerCallback$a.smali
    │   │   │   ├── ChoreographerCallback.smali
    │   │   │   ├── SwappyDisplayManager$1.smali
    │   │   │   ├── SwappyDisplayManager$a.smali
    │   │   │   └── SwappyDisplayManager.smali
    │   │   └── gson
    │   │       ├── DefaultDateTypeAdapter.smali
    │   │       ├── ExclusionStrategy.smali
    │   │       ├── FieldAttributes.smali
    │   │       ├── FieldNamingPolicy$1.smali
    │   │       ├── FieldNamingPolicy$2.smali
    │   │       ├── FieldNamingPolicy$3.smali
    │   │       ├── FieldNamingPolicy$4.smali
    │   │       ├── FieldNamingPolicy$5.smali
    │   │       ├── FieldNamingPolicy.smali
    │   │       ├── FieldNamingStrategy.smali
    │   │       ├── Gson$1.smali
    │   │       ├── Gson$2.smali
    │   │       ├── Gson$3.smali
    │   │       ├── Gson$4.smali
    │   │       ├── Gson$5.smali
    │   │       ├── Gson$6.smali
    │   │       ├── Gson$FutureTypeAdapter.smali
    │   │       ├── Gson.smali
    │   │       ├── GsonBuilder.smali
    │   │       ├── InstanceCreator.smali
    │   │       ├── JsonArray.smali
    │   │       ├── JsonDeserializationContext.smali
    │   │       ├── JsonDeserializer.smali
    │   │       ├── JsonElement.smali
    │   │       ├── JsonIOException.smali
    │   │       ├── JsonNull.smali
    │   │       ├── JsonObject.smali
    │   │       ├── JsonParseException.smali
    │   │       ├── JsonParser.smali
    │   │       ├── JsonPrimitive.smali
    │   │       ├── JsonSerializationContext.smali
    │   │       ├── JsonSerializer.smali
    │   │       ├── JsonStreamParser.smali
    │   │       ├── JsonSyntaxException.smali
    │   │       ├── LongSerializationPolicy$1.smali
    │   │       ├── LongSerializationPolicy$2.smali
    │   │       ├── LongSerializationPolicy.smali
    │   │       ├── TypeAdapter$1.smali
    │   │       ├── TypeAdapter.smali
    │   │       ├── TypeAdapterFactory.smali
    │   │       ├── annotations
    │   │       │   ├── Expose.smali
    │   │       │   ├── JsonAdapter.smali
    │   │       │   ├── SerializedName.smali
    │   │       │   ├── Since.smali
    │   │       │   └── Until.smali
    │   │       ├── internal
    │   │       │   ├── $Gson$Preconditions.smali
    │   │       │   ├── $Gson$Types$GenericArrayTypeImpl.smali
    │   │       │   ├── $Gson$Types$ParameterizedTypeImpl.smali
    │   │       │   ├── $Gson$Types$WildcardTypeImpl.smali
    │   │       │   ├── $Gson$Types.smali
    │   │       │   ├── ConstructorConstructor$1.smali
    │   │       │   ├── ConstructorConstructor$10.smali
    │   │       │   ├── ConstructorConstructor$11.smali
    │   │       │   ├── ConstructorConstructor$12.smali
    │   │       │   ├── ConstructorConstructor$13.smali
    │   │       │   ├── ConstructorConstructor$14.smali
    │   │       │   ├── ConstructorConstructor$2.smali
    │   │       │   ├── ConstructorConstructor$3.smali
    │   │       │   ├── ConstructorConstructor$4.smali
    │   │       │   ├── ConstructorConstructor$5.smali
    │   │       │   ├── ConstructorConstructor$6.smali
    │   │       │   ├── ConstructorConstructor$7.smali
    │   │       │   ├── ConstructorConstructor$8.smali
    │   │       │   ├── ConstructorConstructor$9.smali
    │   │       │   ├── ConstructorConstructor.smali
    │   │       │   ├── Excluder$1.smali
    │   │       │   ├── Excluder.smali
    │   │       │   ├── JsonReaderInternalAccess.smali
    │   │       │   ├── LazilyParsedNumber.smali
    │   │       │   ├── LinkedHashTreeMap$1.smali
    │   │       │   ├── LinkedHashTreeMap$AvlBuilder.smali
    │   │       │   ├── LinkedHashTreeMap$AvlIterator.smali
    │   │       │   ├── LinkedHashTreeMap$EntrySet$1.smali
    │   │       │   ├── LinkedHashTreeMap$EntrySet.smali
    │   │       │   ├── LinkedHashTreeMap$KeySet$1.smali
    │   │       │   ├── LinkedHashTreeMap$KeySet.smali
    │   │       │   ├── LinkedHashTreeMap$LinkedTreeMapIterator.smali
    │   │       │   ├── LinkedHashTreeMap$Node.smali
    │   │       │   ├── LinkedHashTreeMap.smali
    │   │       │   ├── LinkedTreeMap$1.smali
    │   │       │   ├── LinkedTreeMap$EntrySet$1.smali
    │   │       │   ├── LinkedTreeMap$EntrySet.smali
    │   │       │   ├── LinkedTreeMap$KeySet$1.smali
    │   │       │   ├── LinkedTreeMap$KeySet.smali
    │   │       │   ├── LinkedTreeMap$LinkedTreeMapIterator.smali
    │   │       │   ├── LinkedTreeMap$Node.smali
    │   │       │   ├── LinkedTreeMap.smali
    │   │       │   ├── ObjectConstructor.smali
    │   │       │   ├── Primitives.smali
    │   │       │   ├── Streams$AppendableWriter$CurrentWrite.smali
    │   │       │   ├── Streams$AppendableWriter.smali
    │   │       │   ├── Streams.smali
    │   │       │   ├── UnsafeAllocator$1.smali
    │   │       │   ├── UnsafeAllocator$2.smali
    │   │       │   ├── UnsafeAllocator$3.smali
    │   │       │   ├── UnsafeAllocator$4.smali
    │   │       │   ├── UnsafeAllocator.smali
    │   │       │   └── bind
    │   │       │       ├── ArrayTypeAdapter$1.smali
    │   │       │       ├── ArrayTypeAdapter.smali
    │   │       │       ├── CollectionTypeAdapterFactory$Adapter.smali
    │   │       │       ├── CollectionTypeAdapterFactory.smali
    │   │       │       ├── DateTypeAdapter$1.smali
    │   │       │       ├── DateTypeAdapter.smali
    │   │       │       ├── JsonAdapterAnnotationTypeAdapterFactory.smali
    │   │       │       ├── JsonTreeReader$1.smali
    │   │       │       ├── JsonTreeReader.smali
    │   │       │       ├── JsonTreeWriter$1.smali
    │   │       │       ├── JsonTreeWriter.smali
    │   │       │       ├── MapTypeAdapterFactory$Adapter.smali
    │   │       │       ├── MapTypeAdapterFactory.smali
    │   │       │       ├── ObjectTypeAdapter$1.smali
    │   │       │       ├── ObjectTypeAdapter$2.smali
    │   │       │       ├── ObjectTypeAdapter.smali
    │   │       │       ├── ReflectiveTypeAdapterFactory$1.smali
    │   │       │       ├── ReflectiveTypeAdapterFactory$Adapter.smali
    │   │       │       ├── ReflectiveTypeAdapterFactory$BoundField.smali
    │   │       │       ├── ReflectiveTypeAdapterFactory.smali
    │   │       │       ├── SqlDateTypeAdapter$1.smali
    │   │       │       ├── SqlDateTypeAdapter.smali
    │   │       │       ├── TimeTypeAdapter$1.smali
    │   │       │       ├── TimeTypeAdapter.smali
    │   │       │       ├── TreeTypeAdapter$1.smali
    │   │       │       ├── TreeTypeAdapter$GsonContextImpl.smali
    │   │       │       ├── TreeTypeAdapter$SingleTypeFactory.smali
    │   │       │       ├── TreeTypeAdapter.smali
    │   │       │       ├── TypeAdapterRuntimeTypeWrapper.smali
    │   │       │       ├── TypeAdapters$1.smali
    │   │       │       ├── TypeAdapters$10.smali
    │   │       │       ├── TypeAdapters$11.smali
    │   │       │       ├── TypeAdapters$12.smali
    │   │       │       ├── TypeAdapters$13.smali
    │   │       │       ├── TypeAdapters$14.smali
    │   │       │       ├── TypeAdapters$15.smali
    │   │       │       ├── TypeAdapters$16.smali
    │   │       │       ├── TypeAdapters$17.smali
    │   │       │       ├── TypeAdapters$18.smali
    │   │       │       ├── TypeAdapters$19.smali
    │   │       │       ├── TypeAdapters$2.smali
    │   │       │       ├── TypeAdapters$20.smali
    │   │       │       ├── TypeAdapters$21.smali
    │   │       │       ├── TypeAdapters$22.smali
    │   │       │       ├── TypeAdapters$23.smali
    │   │       │       ├── TypeAdapters$24.smali
    │   │       │       ├── TypeAdapters$25.smali
    │   │       │       ├── TypeAdapters$26$1.smali
    │   │       │       ├── TypeAdapters$26.smali
    │   │       │       ├── TypeAdapters$27.smali
    │   │       │       ├── TypeAdapters$28.smali
    │   │       │       ├── TypeAdapters$29.smali
    │   │       │       ├── TypeAdapters$3.smali
    │   │       │       ├── TypeAdapters$30.smali
    │   │       │       ├── TypeAdapters$31.smali
    │   │       │       ├── TypeAdapters$32.smali
    │   │       │       ├── TypeAdapters$33.smali
    │   │       │       ├── TypeAdapters$34.smali
    │   │       │       ├── TypeAdapters$35$1.smali
    │   │       │       ├── TypeAdapters$35.smali
    │   │       │       ├── TypeAdapters$36.smali
    │   │       │       ├── TypeAdapters$4.smali
    │   │       │       ├── TypeAdapters$5.smali
    │   │       │       ├── TypeAdapters$6.smali
    │   │       │       ├── TypeAdapters$7.smali
    │   │       │       ├── TypeAdapters$8.smali
    │   │       │       ├── TypeAdapters$9.smali
    │   │       │       ├── TypeAdapters$EnumTypeAdapter.smali
    │   │       │       ├── TypeAdapters.smali
    │   │       │       └── util
    │   │       │           └── ISO8601Utils.smali
    │   │       ├── reflect
    │   │       │   └── TypeToken.smali
    │   │       └── stream
    │   │           ├── JsonReader$1.smali
    │   │           ├── JsonReader.smali
    │   │           ├── JsonScope.smali
    │   │           ├── JsonToken.smali
    │   │           ├── JsonWriter.smali
    │   │           └── MalformedJsonException.smali
    │   ├── pico
    │   │   └── pbslibrary
    │   │       └── system_attribute
    │   │           ├── IBrightnessManager$Stub$Proxy.smali
    │   │           ├── IBrightnessManager$Stub.smali
    │   │           └── IBrightnessManager.smali
    │   ├── picovr
    │   │   └── picovrlib
    │   │       └── R.smali
    │   ├── psmart
    │   │   └── aosoperation
    │   │       ├── AudioReceiver.smali
    │   │       ├── BatteryReceiver.smali
    │   │       ├── BrightNessReceiver$1.smali
    │   │       ├── BrightNessReceiver.smali
    │   │       ├── MRCCalibration.smali
    │   │       ├── NativeVerfyInterface.smali
    │   │       ├── SysActivity$1.smali
    │   │       ├── SysActivity$2.smali
    │   │       ├── SysActivity$3.smali
    │   │       ├── SysActivity.smali
    │   │       ├── VRResUtils$ResID$ID_BOUNDARY_DIALOG.smali
    │   │       ├── VRResUtils$ResID$ID_LGP_MAIN_SCREEN$IMAGE.smali
    │   │       ├── VRResUtils$ResID$ID_LGP_MAIN_SCREEN$TEXT.smali
    │   │       ├── VRResUtils$ResID$ID_LGP_MAIN_SCREEN.smali
    │   │       ├── VRResUtils$ResID$ID_LGP_PERMISSION_TOAST.smali
    │   │       ├── VRResUtils$ResID.smali
    │   │       ├── VRResUtils.smali
    │   │       ├── VerifyTool$1.smali
    │   │       ├── VerifyTool$2.smali
    │   │       ├── VerifyTool$MyHandler.smali
    │   │       └── VerifyTool.smali
    │   ├── pvr
    │   │   ├── Constants.smali
    │   │   ├── Country.smali
    │   │   ├── IPvrCallback$Stub$Proxy.smali
    │   │   ├── IPvrCallback$Stub.smali
    │   │   ├── IPvrCallback.smali
    │   │   ├── IPvrManagerService$Stub$Proxy.smali
    │   │   ├── IPvrManagerService$Stub.smali
    │   │   ├── IPvrManagerService.smali
    │   │   ├── PvrCallback.smali
    │   │   ├── PvrManager.smali
    │   │   ├── PvrManagerInternal$PvrCallBackTransport.smali
    │   │   ├── PvrManagerInternal.smali
    │   │   ├── PvrXmlUtils.smali
    │   │   ├── tobservice
    │   │   │   ├── ToBServiceHelper$1.smali
    │   │   │   ├── ToBServiceHelper$10.smali
    │   │   │   ├── ToBServiceHelper$11.smali
    │   │   │   ├── ToBServiceHelper$12.smali
    │   │   │   ├── ToBServiceHelper$13.smali
    │   │   │   ├── ToBServiceHelper$14.smali
    │   │   │   ├── ToBServiceHelper$15.smali
    │   │   │   ├── ToBServiceHelper$16.smali
    │   │   │   ├── ToBServiceHelper$17.smali
    │   │   │   ├── ToBServiceHelper$18.smali
    │   │   │   ├── ToBServiceHelper$19.smali
    │   │   │   ├── ToBServiceHelper$2.smali
    │   │   │   ├── ToBServiceHelper$20.smali
    │   │   │   ├── ToBServiceHelper$21.smali
    │   │   │   ├── ToBServiceHelper$22.smali
    │   │   │   ├── ToBServiceHelper$23.smali
    │   │   │   ├── ToBServiceHelper$3.smali
    │   │   │   ├── ToBServiceHelper$4.smali
    │   │   │   ├── ToBServiceHelper$5.smali
    │   │   │   ├── ToBServiceHelper$6.smali
    │   │   │   ├── ToBServiceHelper$7.smali
    │   │   │   ├── ToBServiceHelper$8.smali
    │   │   │   ├── ToBServiceHelper$9.smali
    │   │   │   ├── ToBServiceHelper.smali
    │   │   │   ├── enums
    │   │   │   │   ├── PBS_ControllerKeyEnum$1.smali
    │   │   │   │   ├── PBS_ControllerKeyEnum.smali
    │   │   │   │   ├── PBS_DeviceControlEnum$1.smali
    │   │   │   │   ├── PBS_DeviceControlEnum.smali
    │   │   │   │   ├── PBS_HomeEventEnum$1.smali
    │   │   │   │   ├── PBS_HomeEventEnum.smali
    │   │   │   │   ├── PBS_HomeFunctionEnum$1.smali
    │   │   │   │   ├── PBS_HomeFunctionEnum.smali
    │   │   │   │   ├── PBS_PackageControlEnum$1.smali
    │   │   │   │   ├── PBS_PackageControlEnum.smali
    │   │   │   │   ├── PBS_PowerOnOffLogoEnum$1.smali
    │   │   │   │   ├── PBS_PowerOnOffLogoEnum.smali
    │   │   │   │   ├── PBS_ScreenOffDelayTimeEnum$1.smali
    │   │   │   │   ├── PBS_ScreenOffDelayTimeEnum.smali
    │   │   │   │   ├── PBS_SleepDelayTimeEnum$1.smali
    │   │   │   │   ├── PBS_SleepDelayTimeEnum.smali
    │   │   │   │   ├── PBS_StartVRSettingsEnum$1.smali
    │   │   │   │   ├── PBS_StartVRSettingsEnum.smali
    │   │   │   │   ├── PBS_SwitchEnum$1.smali
    │   │   │   │   ├── PBS_SwitchEnum.smali
    │   │   │   │   ├── PBS_SystemFunctionSwitchEnum$1.smali
    │   │   │   │   ├── PBS_SystemFunctionSwitchEnum.smali
    │   │   │   │   ├── PBS_SystemInfoEnum$1.smali
    │   │   │   │   ├── PBS_SystemInfoEnum.smali
    │   │   │   │   ├── PBS_USBConfigModeEnum$1.smali
    │   │   │   │   ├── PBS_USBConfigModeEnum.smali
    │   │   │   │   ├── PBS_VideoPlayMode$1.smali
    │   │   │   │   ├── PBS_VideoPlayMode.smali
    │   │   │   │   ├── PBS_WifiDisplayModel$1.smali
    │   │   │   │   └── PBS_WifiDisplayModel.smali
    │   │   │   └── interfaces
    │   │   │       ├── IBoolCallback$Stub$Proxy.smali
    │   │   │       ├── IBoolCallback$Stub.smali
    │   │   │       ├── IBoolCallback.smali
    │   │   │       ├── IIntCallback$Stub$Proxy.smali
    │   │   │       ├── IIntCallback$Stub.smali
    │   │   │       ├── IIntCallback.smali
    │   │   │       ├── ILongCallback$Stub$Proxy.smali
    │   │   │       ├── ILongCallback$Stub.smali
    │   │   │       ├── ILongCallback.smali
    │   │   │       ├── IPBS.smali
    │   │   │       ├── ISensorCallback$Stub$Proxy.smali
    │   │   │       ├── ISensorCallback$Stub.smali
    │   │   │       ├── ISensorCallback.smali
    │   │   │       ├── IStringCallback$Stub$Proxy.smali
    │   │   │       ├── IStringCallback$Stub.smali
    │   │   │       ├── IStringCallback.smali
    │   │   │       ├── IToBService$Stub$Proxy.smali
    │   │   │       ├── IToBService$Stub.smali
    │   │   │       ├── IToBService.smali
    │   │   │       ├── IWDJsonCallback$Stub$Proxy.smali
    │   │   │       ├── IWDJsonCallback$Stub.smali
    │   │   │       ├── IWDJsonCallback.smali
    │   │   │       ├── IWDModelsCallback$Stub$Proxy.smali
    │   │   │       ├── IWDModelsCallback$Stub.smali
    │   │   │       ├── IWDModelsCallback.smali
    │   │   │       ├── IWIFIManager$Stub$Proxy.smali
    │   │   │       ├── IWIFIManager$Stub.smali
    │   │   │       └── IWIFIManager.smali
    │   │   └── verify
    │   │       ├── ICallback$Default.smali
    │   │       ├── ICallback$Stub$Proxy.smali
    │   │       ├── ICallback$Stub.smali
    │   │       ├── ICallback.smali
    │   │       ├── IVerify$Default.smali
    │   │       ├── IVerify$Stub$Proxy.smali
    │   │       ├── IVerify$Stub.smali
    │   │       └── IVerify.smali
    │   ├── pxr
    │   │   └── xrlib
    │   │       ├── BuildConfig.smali
    │   │       ├── PicovrSDK.smali
    │   │       └── R.smali
    │   └── unity3d
    │       └── player
    │           ├── AudioVolumeHandler.smali
    │           ├── BuildConfig.smali
    │           ├── Camera2Wrapper.smali
    │           ├── GoogleARCoreApi.smali
    │           ├── GoogleVrApi.smali
    │           ├── GoogleVrProxy$1.smali
    │           ├── GoogleVrProxy$2.smali
    │           ├── GoogleVrProxy$3.smali
    │           ├── GoogleVrProxy$4.smali
    │           ├── GoogleVrProxy$a.smali
    │           ├── GoogleVrProxy.smali
    │           ├── GoogleVrVideo$GoogleVrVideoCallbacks.smali
    │           ├── GoogleVrVideo.smali
    │           ├── HFPStatus$1.smali
    │           ├── HFPStatus$a.smali
    │           ├── HFPStatus.smali
    │           ├── IAssetPackManagerDownloadStatusCallback.smali
    │           ├── IAssetPackManagerMobileDataConfirmationCallback.smali
    │           ├── IAssetPackManagerStatusQueryCallback.smali
    │           ├── IUnityPlayerLifecycleEvents.smali
    │           ├── MultiWindowSupport.smali
    │           ├── NativeLoader.smali
    │           ├── NetworkConnectivity$1.smali
    │           ├── NetworkConnectivity.smali
    │           ├── PlayAssetDeliveryUnityWrapper.smali
    │           ├── R$string.smali
    │           ├── R$style.smali
    │           ├── R.smali
    │           ├── ReflectionHelper$1.smali
    │           ├── ReflectionHelper$a.smali
    │           ├── ReflectionHelper$b.smali
    │           ├── ReflectionHelper.smali
    │           ├── UnityCoreAssetPacksStatusCallbacks.smali
    │           ├── UnityPlayer$1.smali
    │           ├── UnityPlayer$10.smali
    │           ├── UnityPlayer$11.smali
    │           ├── UnityPlayer$12.smali
    │           ├── UnityPlayer$13.smali
    │           ├── UnityPlayer$14.smali
    │           ├── UnityPlayer$15.smali
    │           ├── UnityPlayer$16.smali
    │           ├── UnityPlayer$17.smali
    │           ├── UnityPlayer$18.smali
    │           ├── UnityPlayer$19.smali
    │           ├── UnityPlayer$2.smali
    │           ├── UnityPlayer$20.smali
    │           ├── UnityPlayer$21.smali
    │           ├── UnityPlayer$22.smali
    │           ├── UnityPlayer$23.smali
    │           ├── UnityPlayer$24.smali
    │           ├── UnityPlayer$25.smali
    │           ├── UnityPlayer$26.smali
    │           ├── UnityPlayer$3$1.smali
    │           ├── UnityPlayer$3.smali
    │           ├── UnityPlayer$4$1.smali
    │           ├── UnityPlayer$4.smali
    │           ├── UnityPlayer$5.smali
    │           ├── UnityPlayer$6.smali
    │           ├── UnityPlayer$7.smali
    │           ├── UnityPlayer$8.smali
    │           ├── UnityPlayer$9.smali
    │           ├── UnityPlayer$a.smali
    │           ├── UnityPlayer$b.smali
    │           ├── UnityPlayer$c.smali
    │           ├── UnityPlayer$d.smali
    │           ├── UnityPlayer$e$1.smali
    │           ├── UnityPlayer$e.smali
    │           ├── UnityPlayer$f.smali
    │           ├── UnityPlayer.smali
    │           ├── UnityPlayerActivity.smali
    │           ├── a$a.smali
    │           ├── a$b.smali
    │           ├── a$c$a.smali
    │           ├── a$c.smali
    │           ├── a$d.smali
    │           ├── a$e$a.smali
    │           ├── a$e.smali
    │           ├── a.smali
    │           ├── b$a.smali
    │           ├── b$b.smali
    │           ├── b.smali
    │           ├── c$1.smali
    │           ├── c$2.smali
    │           ├── c$3.smali
    │           ├── c$4.smali
    │           ├── c$5.smali
    │           ├── c$a.smali
    │           ├── c.smali
    │           ├── d$1.smali
    │           ├── d.smali
    │           ├── e.smali
    │           ├── f.smali
    │           ├── g.smali
    │           ├── h.smali
    │           ├── i.smali
    │           ├── j.smali
    │           ├── k.smali
    │           ├── l$a.smali
    │           ├── l.smali
    │           ├── m.smali
    │           ├── n$1.smali
    │           ├── n$2.smali
    │           ├── n$3.smali
    │           ├── n$4.smali
    │           ├── n.smali
    │           ├── o$1.smali
    │           ├── o$a.smali
    │           ├── o.smali
    │           ├── p.smali
    │           ├── q.smali
    │           ├── r$a.smali
    │           ├── r.1.smali
    │           ├── s$a.smali
    │           ├── s$b.smali
    │           ├── s.smali
    │           ├── t$1$1$1.smali
    │           ├── t$1$1.smali
    │           ├── t$1.smali
    │           ├── t$2.smali
    │           ├── t$3.smali
    │           ├── t$4.smali
    │           ├── t$a.smali
    │           └── t.smali
    └── org
        └── fmod
            ├── FMODAudioDevice.smali
            └── a.smali

```


# lib库
与Unity相关的库
* libil2cpp.so
* libmain.so
* libunity.so

其它的库是开发者自己的库。  
```plain
│   └── arm64-v8a
│       ├── libil2cpp.so
│       ├── libmain.so
│       ├── libpxr_api.so
│       ├── libpxrplatformloader.so
│       └── libunity.so
```

# Unity的资源文件打包之后存储到了哪里？
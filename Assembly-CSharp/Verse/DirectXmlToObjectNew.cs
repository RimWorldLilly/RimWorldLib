using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x02000512 RID: 1298
	public static class DirectXmlToObjectNew
	{
		// Token: 0x0600265F RID: 9823 RVA: 0x000EAA44 File Offset: 0x000E8C44
		public static Def DefFromNodeNew(XmlNode node, LoadableXmlAsset loadingAsset)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				return null;
			}
			XmlAttribute xmlAttribute = node.Attributes["Abstract"];
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			XmlNode resolvedNodeFor = XmlInheritance.GetResolvedNodeFor(node);
			string text = node.Name;
			XmlAttribute xmlAttribute2 = resolvedNodeFor.Attributes["Class"];
			if (xmlAttribute2 != null)
			{
				text = xmlAttribute2.Value;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(text, null);
			if (typeInAnyAssembly == null || !GenTypes.IsDef(typeInAnyAssembly))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Type ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" is not a Def type or could not be found, in file ");
				defaultInterpolatedStringHandler.AppendFormatted((loadingAsset != null) ? loadingAsset.name : "(unknown)");
				defaultInterpolatedStringHandler.AppendLiteral(". Context: ");
				defaultInterpolatedStringHandler.AppendFormatted(node.OuterXml);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), text.GetHashCode());
				return null;
			}
			Def def = null;
			try
			{
				DirectXmlToObjectNew.ParseValueAndReturnDefDelegate defParserForType = DirectXmlToObjectNew.GetDefParserForType(typeInAnyAssembly);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("ParseValueAndReturnDef (for ");
				defaultInterpolatedStringHandler2.AppendFormatted<Type>(typeInAnyAssembly);
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				DeepProfiler.Start(defaultInterpolatedStringHandler2.ToStringAndClear());
				def = defParserForType(0, 0, node, typeInAnyAssembly);
				DeepProfiler.End();
				def.ResolveDefNameHash();
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("Exception loading def from file ");
				defaultInterpolatedStringHandler3.AppendFormatted((loadingAsset != null) ? loadingAsset.name : "(unknown)");
				defaultInterpolatedStringHandler3.AppendLiteral(": ");
				defaultInterpolatedStringHandler3.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler3.ToStringAndClear());
			}
			return def;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000EABF0 File Offset: 0x000E8DF0
		public static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate GetFieldSetterForType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type", "Cannot get field setter for null type.");
			}
			if (DirectXmlToObjectNew.TypeCanBeDeserializedWithSharedBody(type))
			{
				type = typeof(object);
			}
			DirectXmlToObjectNew.ParseValueAndSetFieldDelegate parseValueAndSetFieldDelegate;
			if (DirectXmlToObjectNew.parseMethods.TryGetValue(type, out parseValueAndSetFieldDelegate))
			{
				return parseValueAndSetFieldDelegate;
			}
			DeepProfiler.Start("CreateFieldSetterForType");
			parseValueAndSetFieldDelegate = DirectXmlToObjectNew.CreateFieldSetterForType(type);
			DeepProfiler.End();
			DirectXmlToObjectNew.parseMethods[type] = parseValueAndSetFieldDelegate;
			return parseValueAndSetFieldDelegate;
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000EAC60 File Offset: 0x000E8E60
		public static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate GetListItemAdderForType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type", "Cannot get list item adder for null type.");
			}
			if (DirectXmlToObjectNew.TypeCanBeDeserializedWithSharedBody(type))
			{
				type = typeof(object);
			}
			DirectXmlToObjectNew.ParseValueAndAddListItemDelegate parseValueAndAddListItemDelegate;
			if (DirectXmlToObjectNew.parseListItemMethods.TryGetValue(type, out parseValueAndAddListItemDelegate))
			{
				return parseValueAndAddListItemDelegate;
			}
			DeepProfiler.Start("CreateListItemAdderForType");
			parseValueAndAddListItemDelegate = DirectXmlToObjectNew.CreateListItemAdderForType(type);
			DeepProfiler.End();
			DirectXmlToObjectNew.parseListItemMethods[type] = parseValueAndAddListItemDelegate;
			return parseValueAndAddListItemDelegate;
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000EACD0 File Offset: 0x000E8ED0
		public static DirectXmlToObjectNew.ParseValueAndReturnDefDelegate GetDefParserForType(Type type)
		{
			if (DirectXmlToObjectNew.TypeCanBeDeserializedWithSharedBody(type))
			{
				type = typeof(Def);
			}
			DirectXmlToObjectNew.ParseValueAndReturnDefDelegate parseValueAndReturnDefDelegate;
			if (DirectXmlToObjectNew.parseDefMethods.TryGetValue(type, out parseValueAndReturnDefDelegate))
			{
				return parseValueAndReturnDefDelegate;
			}
			parseValueAndReturnDefDelegate = DirectXmlToObjectNew.CreateDefParserForType(type);
			DirectXmlToObjectNew.parseDefMethods[type] = parseValueAndReturnDefDelegate;
			return parseValueAndReturnDefDelegate;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000EAD18 File Offset: 0x000E8F18
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForType(Type type)
		{
			if (XmlToObjectUtils.CustomDataLoadMethodOf(type) != null)
			{
				return DirectXmlToObjectNew.CreateFieldSetterForCustomLoadable(type);
			}
			if (GenTypes.IsSlateRef(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForSlateRef(type);
			}
			if (type == typeof(string))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForString();
			}
			if (GenTypes.HasFlagsAttribute(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForFlagsEnum(type);
			}
			if (GenTypes.IsList(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForList(type);
			}
			if (GenTypes.IsDictionary(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForDict(type);
			}
			return DirectXmlToObjectNew.CreateFieldSetterForGeneralType(type);
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000EAD94 File Offset: 0x000E8F94
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForType(Type type)
		{
			if (XmlToObjectUtils.CustomDataLoadMethodOf(type) != null)
			{
				return DirectXmlToObjectNew.CreateListItemAdderForCustomLoadable(type);
			}
			if (GenTypes.IsSlateRef(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForSlateRef(type);
			}
			if (type == typeof(string))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForString();
			}
			if (GenTypes.HasFlagsAttribute(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForFlagsEnum(type);
			}
			if (GenTypes.IsList(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForList(type);
			}
			if (GenTypes.IsDictionary(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForDict(type);
			}
			return DirectXmlToObjectNew.CreateListItemAdderForGeneralType(type);
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000EAE10 File Offset: 0x000E9010
		private static DirectXmlToObjectNew.ParseValueAndReturnDefDelegate CreateDefParserForType(Type type)
		{
			if (!GenTypes.IsDef(type))
			{
				throw new InvalidOperationException("Cannot create a Def parser for type " + type.FullName + ". Only Def types are supported.");
			}
			string str = (type == typeof(object)) ? "SharedBody" : DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type);
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndReturnDef_" + str, type, new Type[]
			{
				typeof(int),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateComplexType(ilgenerator, type, localBuilder);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndReturnDefDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndReturnDefDelegate));
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000EAEF8 File Offset: 0x000E90F8
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForCustomLoadable(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetCustomLoadableField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateCustomLoadable(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000EAFB0 File Offset: 0x000E91B0
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForSlateRef(Type slateRefType)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetSlateRefField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(slateRefType), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateSlateRef(ilgenerator, slateRefType);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000EB068 File Offset: 0x000E9268
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForString()
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetStringField", null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateString(ilgenerator);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000EB114 File Offset: 0x000E9314
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForFlagsEnum(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetFlagsEnumField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			DirectXmlToObjectNew.EmitIlToHandleSingleTextNodeViaParseHelper(ilgenerator, type, localBuilder, label2, label);
			ilgenerator.MarkLabel(label2);
			DirectXmlToObjectNew.EmitIlToParseFlagsEnum(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Unbox_Any, type);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Box, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000EB22C File Offset: 0x000E942C
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForList(Type listType)
		{
			Type type = listType.GetGenericArguments()[0];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetListField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateList(ilgenerator, listType, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000EB2F0 File Offset: 0x000E94F0
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForDict(Type dictType)
		{
			Type type = dictType.GetGenericArguments()[0];
			Type type2 = dictType.GetGenericArguments()[1];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetDictField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type) + "_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type2), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitILToCreateAndPopulateDictionary(ilgenerator, dictType, type, type2);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000EB3C8 File Offset: 0x000E95C8
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForGeneralType(Type type)
		{
			string str = (type == typeof(object)) ? "SharedBody" : DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type);
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetComplexTypeField_" + str, null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			Type innerIfNullable = type.GetInnerIfNullable();
			bool flag = innerIfNullable != type;
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			LocalBuilder localBuilder2 = flag ? ilgenerator.DeclareLocal(innerIfNullable) : null;
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			Label label4 = ilgenerator.DefineLabel();
			Label label5 = ilgenerator.DefineLabel();
			DirectXmlToObjectNew.EmitIlToHandleEmptyNode(ilgenerator, type, localBuilder, label3, label2);
			ilgenerator.MarkLabel(label3);
			DirectXmlToObjectNew.EmitIlToErrorAndMakeDefaultValueForCdata(ilgenerator, type, label4, label, localBuilder);
			ilgenerator.MarkLabel(label4);
			if (ParseHelper.HandlesType(type))
			{
				DirectXmlToObjectNew.EmitIlToHandleSingleTextNodeViaParseHelper(ilgenerator, innerIfNullable, flag ? localBuilder2 : localBuilder, label5, label);
			}
			ilgenerator.MarkLabel(label5);
			bool flag2 = !innerIfNullable.IsPrimitive && !innerIfNullable.IsEnum && innerIfNullable != typeof(Type);
			bool flag3 = innerIfNullable.IsValueType || innerIfNullable.GetConstructor(Type.EmptyTypes) != null;
			if (flag2 && flag3)
			{
				DirectXmlToObjectNew.EmitIlToCreateAndPopulateComplexType(ilgenerator, innerIfNullable, flag ? localBuilder2 : localBuilder);
			}
			ilgenerator.MarkLabel(label);
			if (flag)
			{
				ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
				ilgenerator.Emit(OpCodes.Newobj, type.GetConstructor(new Type[]
				{
					innerIfNullable
				}));
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			}
			ilgenerator.MarkLabel(label2);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			if (type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, type);
			}
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000EB5FC File Offset: 0x000E97FC
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForCustomLoadable(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddCustomLoadableToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateCustomLoadable(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000EB6B4 File Offset: 0x000E98B4
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForSlateRef(Type slateRefType)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddSlateRefToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(slateRefType), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateSlateRef(ilgenerator, slateRefType);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000EB76C File Offset: 0x000E996C
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForString()
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddStringToList", null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateString(ilgenerator);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000EB818 File Offset: 0x000E9A18
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForFlagsEnum(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddFlagsEnumToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToParseFlagsEnum(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000EB8D0 File Offset: 0x000E9AD0
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForList(Type listType)
		{
			Type type = listType.GetGenericArguments()[0];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddListToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateList(ilgenerator, listType, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000EB994 File Offset: 0x000E9B94
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForDict(Type dictType)
		{
			Type keyType = dictType.GetGenericArguments()[0];
			Type valueType = dictType.GetGenericArguments()[1];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddDictToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(dictType), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitILToCreateAndPopulateDictionary(ilgenerator, dictType, keyType, valueType);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000EBA60 File Offset: 0x000E9C60
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForGeneralType(Type type)
		{
			string str = (type == typeof(object)) ? "SharedBody" : DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type);
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddComplexTypeToList_" + str, null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			Label label4 = ilgenerator.DefineLabel();
			DirectXmlToObjectNew.EmitIlToHandleEmptyNode(ilgenerator, type, localBuilder, label2, label);
			ilgenerator.MarkLabel(label2);
			DirectXmlToObjectNew.EmitIlToErrorAndMakeDefaultValueForCdata(ilgenerator, type, label3, label, localBuilder);
			ilgenerator.MarkLabel(label3);
			if (ParseHelper.HandlesType(type))
			{
				DirectXmlToObjectNew.EmitIlToHandleSingleTextNodeViaParseHelper(ilgenerator, type, localBuilder, label4, label);
			}
			ilgenerator.MarkLabel(label4);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateComplexType(ilgenerator, type, localBuilder);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			if (type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, type);
			}
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000EBBC8 File Offset: 0x000E9DC8
		private static void EmitIlToCreateCustomLoadable(ILGenerator il, Type typeToInstantiate)
		{
			ConstructorInfo constructor = typeToInstantiate.GetConstructor(Type.EmptyTypes);
			MethodInfo methodInfo = XmlToObjectUtils.CustomDataLoadMethodOf(typeToInstantiate);
			if (constructor == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Type ");
				defaultInterpolatedStringHandler.AppendFormatted(typeToInstantiate.FullName);
				defaultInterpolatedStringHandler.AppendLiteral(" does not have a parameterless constructor, but needs one to use ");
				defaultInterpolatedStringHandler.AppendFormatted("LoadDataFromXmlCustom");
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (methodInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(99, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Type ");
				defaultInterpolatedStringHandler2.AppendFormatted(typeToInstantiate.FullName);
				defaultInterpolatedStringHandler2.AppendLiteral(" does not have a method named ");
				defaultInterpolatedStringHandler2.AppendFormatted("LoadDataFromXmlCustom");
				defaultInterpolatedStringHandler2.AppendLiteral(", but we are trying to create a parser for it using that method.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			il.Emit(OpCodes.Newobj, constructor);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.XmlInheritanceGetResolvedNodeForMethod);
			il.Emit(OpCodes.Call, methodInfo);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000EBCE0 File Offset: 0x000E9EE0
		private static void EmitIlToCreateSlateRef(ILGenerator il, Type slateRefType)
		{
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.InnerTextWithReplacedNewlinesOrXmlMethod);
			il.Emit(OpCodes.Ldtoken, slateRefType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000EBD34 File Offset: 0x000E9F34
		private static void EmitIlToCreateString(ILGenerator il)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNodeType));
			LocalBuilder local2 = il.DeclareLocal(typeof(string));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			Label label5 = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeHasChildNodesMethod);
			il.Emit(OpCodes.Brtrue, label);
			il.Emit(OpCodes.Ldstr, "");
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Br, label5);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Beq, label2);
			il.Emit(OpCodes.Ldstr, "XML node has more than one child node, which is unsupported for string parsing. Context: ");
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetOuterXmlMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringConcatMethod);
			il.Emit(OpCodes.Newobj, DirectXmlToObjectNew.InvalidOperationExceptionStringConstructor);
			il.Emit(OpCodes.Throw);
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNodeTypeMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Beq, label3);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Ldc_I4, 4);
			il.Emit(OpCodes.Beq, label4);
			il.Emit(OpCodes.Ldstr, "XML node has an unsupported child node type to parse as string. Expected Text or CDATA. Context: ");
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetOuterXmlMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringConcatMethod);
			il.Emit(OpCodes.Newobj, DirectXmlToObjectNew.InvalidOperationExceptionStringConstructor);
			il.Emit(OpCodes.Throw);
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldtoken, typeof(string));
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
			il.Emit(OpCodes.Castclass, typeof(string));
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Br, label5);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetValueMethod);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Br, label5);
			il.MarkLabel(label5);
			il.Emit(OpCodes.Ldloc, local2);
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000EC03C File Offset: 0x000EA23C
		private static void EmitIlToParseFlagsEnum(ILGenerator il, Type enumType)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local2 = il.DeclareLocal(enumType);
			LocalBuilder local3 = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local4 = il.DeclareLocal(typeof(int));
			LocalBuilder local5 = il.DeclareLocal(typeof(int));
			LocalBuilder local6 = il.DeclareLocal(enumType);
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local5);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Bge, label3);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldtoken, enumType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateListNodeMethod);
			il.Emit(OpCodes.Brfalse, label2);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldtoken, enumType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
			il.Emit(OpCodes.Unbox_Any, enumType);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Or);
			il.Emit(OpCodes.Stloc, local2);
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Box, enumType);
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000EC2F8 File Offset: 0x000EA4F8
		private static void EmitIlToCreateAndPopulateList(ILGenerator il, Type listType, Type itemType)
		{
			ConstructorInfo constructor = listType.GetConstructor(new Type[]
			{
				typeof(int)
			});
			bool flag = GenTypes.IsDef(itemType);
			LocalBuilder local = il.DeclareLocal(listType);
			LocalBuilder local2 = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local3 = il.DeclareLocal(typeof(int));
			LocalBuilder local4 = il.DeclareLocal(typeof(int));
			LocalBuilder local5 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local6 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local7 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local8 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local9 = il.DeclareLocal(typeof(string));
			LocalBuilder local10 = il.DeclareLocal(typeof(string));
			il.DeclareLocal(typeof(XmlAttribute));
			il.DeclareLocal(typeof(string));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			Label label5 = il.DefineLabel();
			Label label6 = il.DefineLabel();
			LocalBuilder local11 = il.DeclareLocal(typeof(Type));
			il.DefineLabel();
			Label loc = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "IsNull");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Ldstr, "true");
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringEqualsWithComparisonModeMethod);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Br, label4);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Newobj, constructor);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local4);
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Bge, label4);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldtoken, itemType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateListNodeMethod);
			il.Emit(OpCodes.Brfalse, label3);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local9);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local10);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequire");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local7);
			il.Emit(OpCodes.Brfalse, label5);
			il.Emit(OpCodes.Ldloc, local7);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Stloc, local9);
			il.MarkLabel(label5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequireAnyOf");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local8);
			il.Emit(OpCodes.Brfalse, label6);
			il.Emit(OpCodes.Ldloc, local8);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Stloc, local10);
			il.MarkLabel(label6);
			if (flag)
			{
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Castclass, listType);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
				il.Emit(OpCodes.Ldarg_2);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNameMethod);
				il.Emit(OpCodes.Box, typeof(string));
				il.Emit(OpCodes.Ldloc, local9);
				il.Emit(OpCodes.Ldloc, local10);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.RegisterListWantsCrossRefMethod.MakeGenericMethod(new Type[]
				{
					itemType
				}));
			}
			else
			{
				il.Emit(OpCodes.Ldloc, local9);
				il.Emit(OpCodes.Ldloc, local10);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateMayRequiresMethod);
				il.Emit(OpCodes.Brfalse, label3);
				il.Emit(OpCodes.Ldtoken, itemType);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveTypeForNodeFromTypeMethod);
				il.Emit(OpCodes.Stloc, local11);
				il.MarkLabel(loc);
				il.Emit(OpCodes.Ldloc, local11);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetListItemAdderForTypeMethod);
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Ldc_I4_0);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldloc, local11);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndAddListItemDelegateInvokeMethod);
			}
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldloc, local);
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000EC99C File Offset: 0x000EAB9C
		private static void EmitILToCreateAndPopulateDictionary(ILGenerator il, Type dictionaryType, Type keyType, Type valueType)
		{
			bool flag = GenTypes.IsDef(keyType);
			bool flag2 = GenTypes.IsDef(valueType);
			bool flag3 = !flag && !flag2;
			Type type = typeof(KeyValuePair<, >).MakeGenericType(new Type[]
			{
				keyType,
				valueType
			});
			MethodInfo getMethod = type.GetProperty("Key").GetGetMethod();
			MethodInfo getMethod2 = type.GetProperty("Value").GetGetMethod();
			ConstructorInfo constructor = dictionaryType.GetConstructor(new Type[]
			{
				typeof(int)
			});
			LocalBuilder local = il.DeclareLocal(dictionaryType);
			LocalBuilder local2 = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local3 = il.DeclareLocal(typeof(int));
			LocalBuilder local4 = il.DeclareLocal(typeof(int));
			LocalBuilder local5 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local6 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local7 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local8 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local9 = il.DeclareLocal(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
			LocalBuilder local10 = il.DeclareLocal(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
			LocalBuilder local11 = il.DeclareLocal(type);
			LocalBuilder local12 = il.DeclareLocal(typeof(object));
			LocalBuilder local13 = il.DeclareLocal(typeof(FieldInfo));
			LocalBuilder local14 = il.DeclareLocal(typeof(FieldInfo));
			LocalBuilder local15 = il.DeclareLocal(typeof(Type));
			LocalBuilder local16 = il.DeclareLocal(typeof(Type));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			if (flag3)
			{
				il.Emit(OpCodes.Ldloca, local11);
				il.Emit(OpCodes.Initobj, type);
				il.Emit(OpCodes.Ldtoken, type);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldstr, "key");
				il.Emit(OpCodes.Ldc_I4, 36);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.TypeGetFieldMethod);
				il.Emit(OpCodes.Stloc, local13);
				il.Emit(OpCodes.Ldtoken, type);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldstr, "value");
				il.Emit(OpCodes.Ldc_I4, 36);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.TypeGetFieldMethod);
				il.Emit(OpCodes.Stloc, local14);
			}
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "IsNull");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local8);
			il.Emit(OpCodes.Brfalse, label4);
			il.Emit(OpCodes.Ldloc, local8);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Ldstr, "true");
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringEqualsWithComparisonModeMethod);
			il.Emit(OpCodes.Brfalse, label4);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Br, label3);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Newobj, constructor);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local4);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Bge, label3);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldtoken, type);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateListNodeMethod);
			il.Emit(OpCodes.Brfalse, label2);
			if (flag3)
			{
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldstr, "key");
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetItemByNameMethod);
				il.Emit(OpCodes.Stloc, local6);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldstr, "value");
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetItemByNameMethod);
				il.Emit(OpCodes.Stloc, local7);
				il.Emit(OpCodes.Ldloc, local11);
				il.Emit(OpCodes.Box, type);
				il.Emit(OpCodes.Stloc, local12);
				il.Emit(OpCodes.Ldtoken, keyType);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldloc, local6);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveTypeForNodeFromTypeMethod);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, local15);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetFieldSetterForTypeMethod);
				il.Emit(OpCodes.Stloc, local9);
				il.Emit(OpCodes.Ldloc, local9);
				il.Emit(OpCodes.Ldloc, local12);
				il.Emit(OpCodes.Ldloc, local13);
				il.Emit(OpCodes.Ldloc, local6);
				il.Emit(OpCodes.Ldloc, local15);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndSetFieldDelegateInvokeMethod);
				il.Emit(OpCodes.Ldtoken, valueType);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldloc, local7);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveTypeForNodeFromTypeMethod);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, local16);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetFieldSetterForTypeMethod);
				il.Emit(OpCodes.Stloc, local10);
				il.Emit(OpCodes.Ldloc, local10);
				il.Emit(OpCodes.Ldloc, local12);
				il.Emit(OpCodes.Ldloc, local14);
				il.Emit(OpCodes.Ldloc, local7);
				il.Emit(OpCodes.Ldloc, local16);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndSetFieldDelegateInvokeMethod);
				il.Emit(OpCodes.Ldloc, local12);
				il.Emit(OpCodes.Unbox_Any, type);
				il.Emit(OpCodes.Stloc, local11);
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Ldloca, local11);
				il.Emit(OpCodes.Call, getMethod);
				if (keyType.IsValueType)
				{
					il.Emit(OpCodes.Box, keyType);
				}
				il.Emit(OpCodes.Ldloca, local11);
				il.Emit(OpCodes.Call, getMethod2);
				if (valueType.IsValueType)
				{
					il.Emit(OpCodes.Box, valueType);
				}
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.DictionaryAddMethod);
			}
			else
			{
				MethodInfo meth = DirectXmlToObjectNew.RegisterDictionaryWantsCrossRefMethod.MakeGenericMethod(new Type[]
				{
					keyType,
					valueType
				});
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldarg_2);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNameMethod);
				il.Emit(OpCodes.Call, meth);
			}
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldloc, local);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000ED1B0 File Offset: 0x000EB3B0
		private static void EmitIlToCreateAndPopulateComplexType(ILGenerator il, Type typeBeingDeserialized, LocalBuilder instantiatedObjectLocal)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local2 = il.DeclareLocal(typeof(int));
			LocalBuilder local3 = il.DeclareLocal(typeof(int));
			LocalBuilder local4 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local5 = il.DeclareLocal(typeof(FieldInfo));
			LocalBuilder local6 = il.DeclareLocal(typeof(Type));
			LocalBuilder localBuilder = il.DeclareLocal(typeof(object));
			LocalBuilder local7 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local8 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local9 = il.DeclareLocal(typeof(string));
			LocalBuilder local10 = il.DeclareLocal(typeof(string));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			Label label5 = il.DefineLabel();
			Label label6 = il.DefineLabel();
			Label label7 = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.XmlInheritanceGetResolvedNodeForMethod);
			il.Emit(OpCodes.Starg_S, 2);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Stloc, local2);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloca, instantiatedObjectLocal);
				il.Emit(OpCodes.Initobj, typeBeingDeserialized);
			}
			else
			{
				il.Emit(OpCodes.Ldarg_3);
				il.Emit(OpCodes.Ldarg_2);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.MakeInstanceOfTypeForEmptyNodeMethod);
				il.Emit(OpCodes.Stloc, instantiatedObjectLocal);
			}
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local3);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Bge, label3);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloc, instantiatedObjectLocal);
				il.Emit(OpCodes.Box, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, localBuilder);
			}
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Isinst, typeof(XmlComment));
			il.Emit(OpCodes.Brtrue, label2);
			il.Emit(OpCodes.Ldarg_3);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveFieldForNodeMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Brfalse, label2);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveTypeForNodeFromFieldMethod);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GenTypesIsDefMethod);
			il.Emit(OpCodes.Brfalse, label4);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GenTextIsNullOrEmptyMethod);
			il.Emit(OpCodes.Brfalse, label5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, typeBeingDeserialized.IsValueType ? localBuilder : instantiatedObjectLocal);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label5);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local9);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local10);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequire");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local7);
			il.Emit(OpCodes.Brfalse, label6);
			il.Emit(OpCodes.Ldloc, local7);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringToLowerMethod);
			il.Emit(OpCodes.Stloc, local9);
			il.MarkLabel(label6);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequireAnyOf");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local8);
			il.Emit(OpCodes.Brfalse, label7);
			il.Emit(OpCodes.Ldloc, local8);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringToLowerMethod);
			il.Emit(OpCodes.Stloc, local10);
			il.MarkLabel(label7);
			il.Emit(OpCodes.Ldloc, typeBeingDeserialized.IsValueType ? localBuilder : instantiatedObjectLocal);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldloc, local9);
			il.Emit(OpCodes.Ldloc, local10);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.RegisterObjectWantsCrossRefMethod);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetFieldSetterForTypeMethod);
			il.Emit(OpCodes.Ldloc, typeBeingDeserialized.IsValueType ? localBuilder : instantiatedObjectLocal);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndSetFieldDelegateInvokeMethod);
			il.MarkLabel(label2);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloc, localBuilder);
				il.Emit(OpCodes.Unbox_Any, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, instantiatedObjectLocal);
			}
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			MethodInfo methodInfo = XmlToObjectUtils.PostLoadMethodOf(typeBeingDeserialized);
			if (methodInfo != null)
			{
				il.Emit(OpCodes.Ldloc, instantiatedObjectLocal);
				il.Emit(OpCodes.Call, methodInfo);
				return;
			}
			il.Emit(OpCodes.Nop);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000ED8B0 File Offset: 0x000EBAB0
		private static void EmitIlToHandleEmptyNode(ILGenerator il, Type typeBeingDeserialized, LocalBuilder localForResultValue, Label labelToJumpIfNotEmpty, Label endLabel)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlAttribute));
			Label label = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "IsNull");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Ldstr, "true");
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringEqualsWithComparisonModeMethod);
			il.Emit(OpCodes.Brfalse, label);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloca, localForResultValue);
				il.Emit(OpCodes.Initobj, typeBeingDeserialized);
			}
			else
			{
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			il.Emit(OpCodes.Br, endLabel);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeHasChildNodesMethod);
			il.Emit(OpCodes.Brtrue, labelToJumpIfNotEmpty);
			il.Emit(OpCodes.Ldarg_3);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.MakeInstanceOfTypeForEmptyNodeMethod);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, typeBeingDeserialized);
			}
			il.Emit(OpCodes.Stloc, localForResultValue);
			il.Emit(OpCodes.Br, endLabel);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000EDA50 File Offset: 0x000EBC50
		private static void EmitIlToErrorAndMakeDefaultValueForCdata(ILGenerator il, Type typeBeingDeserialized, Label labelToJumpIfNotCdata, Label labelToJumpAfterCheck, LocalBuilder localForResultValue)
		{
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Brfalse, labelToJumpIfNotCdata);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNodeTypeMethod);
			il.Emit(OpCodes.Ldc_I4, 4);
			il.Emit(OpCodes.Bne_Un, labelToJumpIfNotCdata);
			il.Emit(OpCodes.Ldstr, "CDATA can only be used for strings. Bad xml: ");
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetOuterXmlMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringConcatMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.LogErrorMethod);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloca_S, localForResultValue);
				il.Emit(OpCodes.Initobj, typeBeingDeserialized);
			}
			else
			{
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			il.Emit(OpCodes.Br, labelToJumpAfterCheck);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000EDB5C File Offset: 0x000EBD5C
		private static void EmitIlToHandleSingleTextNodeViaParseHelper(ILGenerator il, Type typeBeingDeserialized, LocalBuilder localForResultValue, Label labelToJumpIfNotText, Label endLabel)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNode));
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetNodeOnlyChildMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Brfalse, labelToJumpIfNotText);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNodeTypeMethod);
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Bne_Un, labelToJumpIfNotText);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldarg_3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			else
			{
				il.Emit(OpCodes.Castclass, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			il.Emit(OpCodes.Br, endLabel);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000EDC69 File Offset: 0x000EBE69
		public static FieldInfo ResolveFieldForNode(Type typeBeingDeserialized, XmlNode node, XmlNode parentForDebug)
		{
			return XmlToObjectUtils.DoFieldSearch(typeBeingDeserialized.GetInnerIfNullable(), node, parentForDebug);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000EDC78 File Offset: 0x000EBE78
		public static Type ResolveTypeForNode(FieldInfo fieldBeingSet, XmlNode node)
		{
			return DirectXmlToObjectNew.ResolveTypeForNode(fieldBeingSet.FieldType, node);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000EDC88 File Offset: 0x000EBE88
		public static Type ResolveTypeForNode(Type defaultType, XmlNode node)
		{
			XmlAttributeCollection attributes = node.Attributes;
			XmlAttribute xmlAttribute = (attributes != null) ? attributes["Class"] : null;
			if (xmlAttribute == null)
			{
				return defaultType;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlAttribute.Value, defaultType.GetInnerIfNullable().Namespace);
			if (typeInAnyAssembly != null)
			{
				return typeInAnyAssembly;
			}
			throw new ArgumentException("Could not find type named " + xmlAttribute.Value + " from node " + node.OuterXml, "node");
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000EDCFC File Offset: 0x000EBEFC
		public static XmlNode GetNodeOnlyChild(XmlNode node)
		{
			XmlNode firstChild = node.FirstChild;
			if (firstChild == null || firstChild.NextSibling != null)
			{
				return null;
			}
			return firstChild;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000EDD20 File Offset: 0x000EBF20
		public static object MakeInstanceOfTypeForEmptyNode(Type type, XmlNode node)
		{
			type = DirectXmlToObjectNew.ResolveTypeForNode(type, node);
			if (type.IsValueType)
			{
				return RuntimeHelpers.GetUninitializedObject(type);
			}
			object result;
			try
			{
				result = Activator.CreateInstance(type);
			}
			catch (MissingMethodException innerException)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot deserialize XML node ");
				defaultInterpolatedStringHandler.AppendFormatted(node.OuterXml);
				defaultInterpolatedStringHandler.AppendLiteral(" to type ");
				defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral(" due to missing parameterless constructor.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
			}
			return result;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000EDDB8 File Offset: 0x000EBFB8
		public static bool ValidateMayRequires(string mayRequire, string mayRequireAny)
		{
			if (!mayRequire.NullOrEmpty() && !ModLister.AllModsActiveNoSuffix(mayRequire.Split(',', StringSplitOptions.None)))
			{
				if (DirectXmlCrossRefLoader.MistypedMayRequire(mayRequire))
				{
					Log.Error("Faulty MayRequire: " + mayRequire);
				}
				return false;
			}
			return mayRequireAny.NullOrEmpty() || ModLister.AnyModActiveNoSuffix(mayRequireAny.Split(',', StringSplitOptions.None));
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000EDE12 File Offset: 0x000EC012
		private static string GetDynamicMethodNameSuffixForType(Type type)
		{
			string fullName = type.FullName;
			return ((fullName != null) ? fullName.Replace('.', '_') : null) ?? "UnknownType";
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000EDE33 File Offset: 0x000EC033
		private static Type GetInnerIfNullable(this Type type)
		{
			return Nullable.GetUnderlyingType(type) ?? type;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000EDE40 File Offset: 0x000EC040
		private static bool TypeCanBeDeserializedWithSharedBody(Type type)
		{
			if (type.IsValueType)
			{
				return false;
			}
			if (type.IsGenericType || type.IsConstructedGenericType)
			{
				return false;
			}
			if (XmlToObjectUtils.CustomDataLoadMethodOf(type) != null)
			{
				return false;
			}
			MethodInfo methodInfo = XmlToObjectUtils.PostLoadMethodOf(type);
			return (methodInfo == null || !(methodInfo.DeclaringType != typeof(Editable))) && !ParseHelper.HandlesType(type);
		}

		// Token: 0x04001B8D RID: 7053
		private static readonly MethodInfo XmlNodeGetChildNodesMethod = typeof(XmlNode).GetProperty("ChildNodes").GetGetMethod();

		// Token: 0x04001B8E RID: 7054
		private static readonly MethodInfo XmlNodeGetInnerTextMethod = typeof(XmlNode).GetProperty("InnerText").GetGetMethod();

		// Token: 0x04001B8F RID: 7055
		private static readonly MethodInfo XmlNodeGetFirstChildMethod = typeof(XmlNode).GetProperty("FirstChild").GetGetMethod();

		// Token: 0x04001B90 RID: 7056
		private static readonly MethodInfo XmlNodeHasChildNodesMethod = typeof(XmlNode).GetProperty("HasChildNodes").GetGetMethod();

		// Token: 0x04001B91 RID: 7057
		private static readonly MethodInfo XmlNodeGetOuterXmlMethod = typeof(XmlNode).GetProperty("OuterXml").GetGetMethod();

		// Token: 0x04001B92 RID: 7058
		private static readonly MethodInfo XmlNodeGetNodeTypeMethod = typeof(XmlNode).GetProperty("NodeType").GetGetMethod();

		// Token: 0x04001B93 RID: 7059
		private static readonly MethodInfo XmlNodeGetValueMethod = typeof(XmlNode).GetProperty("Value").GetGetMethod();

		// Token: 0x04001B94 RID: 7060
		private static readonly MethodInfo XmlNodeGetNameMethod = typeof(XmlNode).GetProperty("Name").GetGetMethod();

		// Token: 0x04001B95 RID: 7061
		private static readonly MethodInfo XmlNodeGetAttributesMethod = typeof(XmlNode).GetProperty("Attributes").GetGetMethod();

		// Token: 0x04001B96 RID: 7062
		private static readonly MethodInfo XmlNodeGetItemByNameMethod = typeof(XmlNode).GetMethod("get_Item", new Type[]
		{
			typeof(string)
		});

		// Token: 0x04001B97 RID: 7063
		private static readonly MethodInfo XmlNodeListGetCountMethod = typeof(XmlNodeList).GetProperty("Count").GetGetMethod();

		// Token: 0x04001B98 RID: 7064
		private static readonly MethodInfo XmlNodeListGetItemMethod = typeof(XmlNodeList).GetMethod("Item", new Type[]
		{
			typeof(int)
		});

		// Token: 0x04001B99 RID: 7065
		private static readonly MethodInfo XmlAttributeCollectionGetItemByNameMethod = typeof(XmlAttributeCollection).GetMethod("get_ItemOf", new Type[]
		{
			typeof(string)
		});

		// Token: 0x04001B9A RID: 7066
		private static readonly MethodInfo XmlAttributeGetValueMethod = typeof(XmlAttribute).GetProperty("Value").GetGetMethod();

		// Token: 0x04001B9B RID: 7067
		private static readonly MethodInfo FieldSetValueMethod = typeof(FieldInfo).GetMethod("SetValue", new Type[]
		{
			typeof(object),
			typeof(object)
		});

		// Token: 0x04001B9C RID: 7068
		private static readonly MethodInfo IListAddItemMethod = typeof(IList).GetMethod("Add", new Type[]
		{
			typeof(object)
		});

		// Token: 0x04001B9D RID: 7069
		private static readonly MethodInfo ValidateListNodeMethod = typeof(DirectXmlToObject).GetMethod("ValidateListNode", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001B9E RID: 7070
		private static readonly MethodInfo ParseHelperFromStringNonGenericMethod = typeof(ParseHelper).GetMethod("FromString", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(string),
			typeof(Type)
		}, null);

		// Token: 0x04001B9F RID: 7071
		private static readonly MethodInfo InnerTextWithReplacedNewlinesOrXmlMethod = typeof(DirectXmlToObject).GetMethod("InnerTextWithReplacedNewlinesOrXML", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BA0 RID: 7072
		private static readonly MethodInfo LogMessageMethod = typeof(Log).GetMethod("Message", new Type[]
		{
			typeof(string)
		});

		// Token: 0x04001BA1 RID: 7073
		private static readonly MethodInfo LogErrorMethod = typeof(Log).GetMethod("Error", new Type[]
		{
			typeof(string)
		});

		// Token: 0x04001BA2 RID: 7074
		private static readonly MethodInfo TypeGetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");

		// Token: 0x04001BA3 RID: 7075
		private static readonly MethodInfo TypeGetFieldMethod = typeof(Type).GetMethod("GetField", new Type[]
		{
			typeof(string),
			typeof(BindingFlags)
		});

		// Token: 0x04001BA4 RID: 7076
		private static readonly MethodInfo StringConcatMethod = typeof(string).GetMethod("Concat", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x04001BA5 RID: 7077
		private static readonly MethodInfo StringConcatObjObjMethod = typeof(string).GetMethod("Concat", new Type[]
		{
			typeof(object),
			typeof(object)
		});

		// Token: 0x04001BA6 RID: 7078
		private static readonly MethodInfo StringToLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

		// Token: 0x04001BA7 RID: 7079
		private static readonly MethodInfo StringEqualsWithComparisonModeMethod = typeof(string).GetMethod("Equals", new Type[]
		{
			typeof(string),
			typeof(StringComparison)
		});

		// Token: 0x04001BA8 RID: 7080
		private static readonly MethodInfo XmlInheritanceGetResolvedNodeForMethod = typeof(XmlInheritance).GetMethod("GetResolvedNodeFor", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BA9 RID: 7081
		private static readonly MethodInfo DictionaryAddMethod = typeof(IDictionary).GetMethod("Add");

		// Token: 0x04001BAA RID: 7082
		private static readonly MethodInfo RegisterObjectWantsCrossRefMethod = typeof(DirectXmlCrossRefLoader).GetMethod("RegisterObjectWantsCrossRef", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(object),
			typeof(FieldInfo),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(Type)
		}, null);

		// Token: 0x04001BAB RID: 7083
		private static readonly MethodInfo RegisterDictionaryWantsCrossRefMethod = typeof(DirectXmlCrossRefLoader).GetMethods(BindingFlags.Static | BindingFlags.Public).First((MethodInfo m) => m.Name == "RegisterDictionaryWantsCrossRef");

		// Token: 0x04001BAC RID: 7084
		private static readonly MethodInfo RegisterListWantsCrossRefMethod = typeof(DirectXmlCrossRefLoader).GetMethods(BindingFlags.Static | BindingFlags.Public).First((MethodInfo m) => m.Name == "RegisterListWantsCrossRef");

		// Token: 0x04001BAD RID: 7085
		private static readonly MethodInfo GenTypesIsDefMethod = typeof(GenTypes).GetMethod("IsDef", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BAE RID: 7086
		private static readonly MethodInfo GenTypesGetTypeInAnyAssemblyMethod = typeof(GenTypes).GetMethod("GetTypeInAnyAssembly", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BAF RID: 7087
		private static readonly MethodInfo GenTextIsNullOrEmptyMethod = typeof(GenText).GetMethod("NullOrEmpty", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BB0 RID: 7088
		private static readonly MethodInfo GetFieldSetterForTypeMethod = typeof(DirectXmlToObjectNew).GetMethod("GetFieldSetterForType", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BB1 RID: 7089
		private static readonly MethodInfo GetListItemAdderForTypeMethod = typeof(DirectXmlToObjectNew).GetMethod("GetListItemAdderForType", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BB2 RID: 7090
		private static readonly MethodInfo ResolveFieldForNodeMethod = typeof(DirectXmlToObjectNew).GetMethod("ResolveFieldForNode", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BB3 RID: 7091
		private static readonly MethodInfo ResolveTypeForNodeFromFieldMethod = typeof(DirectXmlToObjectNew).GetMethod("ResolveTypeForNode", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(FieldInfo),
			typeof(XmlNode)
		}, null);

		// Token: 0x04001BB4 RID: 7092
		private static readonly MethodInfo ResolveTypeForNodeFromTypeMethod = typeof(DirectXmlToObjectNew).GetMethod("ResolveTypeForNode", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(Type),
			typeof(XmlNode)
		}, null);

		// Token: 0x04001BB5 RID: 7093
		private static readonly MethodInfo MakeInstanceOfTypeForEmptyNodeMethod = typeof(DirectXmlToObjectNew).GetMethod("MakeInstanceOfTypeForEmptyNode", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BB6 RID: 7094
		private static readonly MethodInfo GetNodeOnlyChildMethod = typeof(DirectXmlToObjectNew).GetMethod("GetNodeOnlyChild", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BB7 RID: 7095
		private static readonly MethodInfo ValidateMayRequiresMethod = typeof(DirectXmlToObjectNew).GetMethod("ValidateMayRequires", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04001BB8 RID: 7096
		private static readonly MethodInfo ParseValueAndSetFieldDelegateInvokeMethod = typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate).GetMethod("Invoke");

		// Token: 0x04001BB9 RID: 7097
		private static readonly MethodInfo ParseValueAndAddListItemDelegateInvokeMethod = typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate).GetMethod("Invoke");

		// Token: 0x04001BBA RID: 7098
		private static readonly MethodInfo ParseValueAndReturnDefDelegateInvokeMethod = typeof(DirectXmlToObjectNew.ParseValueAndReturnDefDelegate).GetMethod("Invoke");

		// Token: 0x04001BBB RID: 7099
		private static readonly ConstructorInfo InvalidOperationExceptionStringConstructor = typeof(InvalidOperationException).GetConstructor(new Type[]
		{
			typeof(string)
		});

		// Token: 0x04001BBC RID: 7100
		private static readonly Dictionary<Type, DirectXmlToObjectNew.ParseValueAndSetFieldDelegate> parseMethods = new Dictionary<Type, DirectXmlToObjectNew.ParseValueAndSetFieldDelegate>();

		// Token: 0x04001BBD RID: 7101
		private static readonly Dictionary<Type, DirectXmlToObjectNew.ParseValueAndAddListItemDelegate> parseListItemMethods = new Dictionary<Type, DirectXmlToObjectNew.ParseValueAndAddListItemDelegate>();

		// Token: 0x04001BBE RID: 7102
		private static readonly Dictionary<Type, DirectXmlToObjectNew.ParseValueAndReturnDefDelegate> parseDefMethods = new Dictionary<Type, DirectXmlToObjectNew.ParseValueAndReturnDefDelegate>();

		// Token: 0x0200275F RID: 10079
		// (Invoke) Token: 0x0600F7F5 RID: 63477
		public delegate void ParseValueAndSetFieldDelegate(object target, FieldInfo field, XmlNode node, Type typeBeingDeserialized);

		// Token: 0x02002760 RID: 10080
		// (Invoke) Token: 0x0600F7F9 RID: 63481
		public delegate void ParseValueAndAddListItemDelegate(IList target, int unused, XmlNode node, Type itemType);

		// Token: 0x02002761 RID: 10081
		// (Invoke) Token: 0x0600F7FD RID: 63485
		public delegate Def ParseValueAndReturnDefDelegate(int unused, int unused2, XmlNode node, Type defType);

		// Token: 0x02002762 RID: 10082
		private class DummyTypeToHoldDynamicMethods
		{
		}
	}
}

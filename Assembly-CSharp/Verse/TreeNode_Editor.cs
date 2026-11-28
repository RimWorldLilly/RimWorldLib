using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000622 RID: 1570
	public class TreeNode_Editor : TreeNode
	{
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x00139F70 File Offset: 0x00138170
		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)this.parentNode).obj;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x00139F84 File Offset: 0x00138184
		public Type ObjectType
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.FieldType;
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetGenericArguments()[0];
				}
				if (this.obj != null)
				{
					return this.obj.GetType();
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x00139FDF File Offset: 0x001381DF
		public bool HasValue
		{
			get
			{
				return this.owningField != null || this.IsListItem;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x00139FFC File Offset: 0x001381FC
		// (set) Token: 0x06003117 RID: 12567 RVA: 0x0013A06C File Offset: 0x0013826C
		public object Value
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.GetValue(this.ParentObj);
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetProperty("Item").GetValue(this.ListRootObject, new object[]
					{
						this.owningIndex
					});
				}
				throw new InvalidOperationException();
			}
			set
			{
				if (this.owningField != null)
				{
					this.owningField.SetValue(this.ParentObj, value);
				}
				if (this.IsListItem)
				{
					this.ListRootObject.GetType().GetProperty("Item").SetValue(this.ListRootObject, value, new object[]
					{
						this.owningIndex
					});
				}
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003118 RID: 12568 RVA: 0x0013A0D6 File Offset: 0x001382D6
		public bool IsListItem
		{
			get
			{
				return this.owningIndex >= 0;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003119 RID: 12569 RVA: 0x0013A0E4 File Offset: 0x001382E4
		private object ListRootObject
		{
			get
			{
				return this.ParentObj;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x0600311A RID: 12570 RVA: 0x0013A0EC File Offset: 0x001382EC
		public override bool Openable
		{
			get
			{
				return this.obj != null && this.nodeType != EditTreeNodeType.TerminalValue && (this.nodeType != EditTreeNodeType.ListRoot || (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null) != 0);
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600311B RID: 12571 RVA: 0x0013A142 File Offset: 0x00138342
		public bool HasContentLines
		{
			get
			{
				return this.nodeType != EditTreeNodeType.TerminalValue;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600311C RID: 12572 RVA: 0x0013A150 File Offset: 0x00138350
		public bool HasNewButton
		{
			get
			{
				return (this.nodeType == EditTreeNodeType.ComplexObject && this.obj == null) || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>());
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600311D RID: 12573 RVA: 0x0013A187 File Offset: 0x00138387
		public bool HasDeleteButton
		{
			get
			{
				return this.IsListItem || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>());
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600311E RID: 12574 RVA: 0x0013A1B8 File Offset: 0x001383B8
		public string ExtraInfoText
		{
			get
			{
				if (this.obj == null)
				{
					return "null";
				}
				if (this.obj.GetType().HasAttribute<EditorShowClassNameAttribute>())
				{
					return this.obj.GetType().Name;
				}
				if (this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null);
					return string.Concat(new string[]
					{
						"(",
						num.ToString(),
						" ",
						(num == 1) ? "element" : "elements",
						")"
					});
				}
				return "";
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x0013A299 File Offset: 0x00138499
		public string LabelText
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.Name;
				}
				if (this.IsListItem)
				{
					return this.owningIndex.ToString();
				}
				return this.ObjectType.Name;
			}
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x0013A2D4 File Offset: 0x001384D4
		private TreeNode_Editor()
		{
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x0013A2EA File Offset: 0x001384EA
		public static TreeNode_Editor NewRootNode(object rootObj)
		{
			if (rootObj.GetType().IsValueEditable())
			{
				throw new ArgumentException();
			}
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.owningField = null;
			treeNode_Editor.obj = rootObj;
			treeNode_Editor.nestDepth = 0;
			treeNode_Editor.RebuildChildNodes();
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x0013A328 File Offset: 0x00138528
		public static TreeNode_Editor NewChildNodeFromField(TreeNode_Editor parent, FieldInfo fieldInfo)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningField = fieldInfo;
			if (!fieldInfo.FieldType.IsValueEditable())
			{
				treeNode_Editor.obj = fieldInfo.GetValue(parent.obj);
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x0013A384 File Offset: 0x00138584
		private static TreeNode_Editor NewChildNodeFromListItem(TreeNode_Editor parent, int listIndex)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningIndex = listIndex;
			object obj = parent.obj;
			Type type = obj.GetType();
			if (!type.GetGenericArguments()[0].IsValueEditable())
			{
				object value = type.GetProperty("Item").GetValue(obj, new object[]
				{
					listIndex
				});
				treeNode_Editor.obj = value;
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x0013A408 File Offset: 0x00138608
		private void InitiallyCacheData()
		{
			if (this.obj != null && this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
			{
				this.nodeType = EditTreeNodeType.ListRoot;
			}
			else if (this.ObjectType.IsValueEditable())
			{
				this.nodeType = EditTreeNodeType.TerminalValue;
			}
			else
			{
				this.nodeType = EditTreeNodeType.ComplexObject;
			}
			if (this.obj != null)
			{
				this.editWidgetsMethod = this.obj.GetType().GetMethod("DoEditWidgets");
			}
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x0013A49C File Offset: 0x0013869C
		public void RebuildChildNodes()
		{
			if (this.obj == null)
			{
				return;
			}
			this.children = new List<TreeNode>();
			Type objType = this.obj.GetType();
			if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
			{
				int num = (int)objType.GetProperty("Count").GetValue(this.obj, null);
				for (int i = 0; i < num; i++)
				{
					TreeNode_Editor item = TreeNode_Editor.NewChildNodeFromListItem(this, i);
					this.children.Add(item);
				}
				return;
			}
			IEnumerable<FieldInfo> fields = this.obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Func<FieldInfo, int> <>9__0;
			Func<FieldInfo, int> keySelector;
			if ((keySelector = <>9__0) == null)
			{
				keySelector = (<>9__0 = ((FieldInfo f) => this.InheritanceDistanceBetween(objType, f.DeclaringType)));
			}
			foreach (FieldInfo fieldInfo in fields.OrderByDescending(keySelector))
			{
				if (fieldInfo.GetCustomAttributes(typeof(UnsavedAttribute), true).Length == 0 && fieldInfo.GetCustomAttributes(typeof(EditorHiddenAttribute), true).Length == 0 && !fieldInfo.IsNotSerialized)
				{
					TreeNode_Editor item2 = TreeNode_Editor.NewChildNodeFromField(this, fieldInfo);
					this.children.Add(item2);
				}
			}
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x0013A604 File Offset: 0x00138804
		private int InheritanceDistanceBetween(Type childType, Type parentType)
		{
			Type type = childType;
			int num = 0;
			while (!(type == parentType))
			{
				type = type.BaseType;
				num++;
				if (type == null)
				{
					Log.Error(((childType != null) ? childType.ToString() : null) + " is not a subclass of " + ((parentType != null) ? parentType.ToString() : null));
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x0013A660 File Offset: 0x00138860
		public void CheckLatentDelete()
		{
			if (this.indexToDelete >= 0)
			{
				this.obj.GetType().GetMethod("RemoveAt").Invoke(this.obj, new object[]
				{
					this.indexToDelete
				});
				this.RebuildChildNodes();
				this.indexToDelete = -1;
			}
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x0013A6B8 File Offset: 0x001388B8
		public void Delete()
		{
			if (this.owningField != null)
			{
				this.owningField.SetValue(this.obj, null);
				return;
			}
			if (this.IsListItem)
			{
				((TreeNode_Editor)this.parentNode).indexToDelete = this.owningIndex;
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x0013A70C File Offset: 0x0013890C
		public void DoSpecialPreElements(Listing_TreeDefs listing)
		{
			if (this.obj == null)
			{
				return;
			}
			if (this.editWidgetsMethod != null)
			{
				WidgetRow widgetRow = listing.StartWidgetsRow(this.nestDepth);
				this.editWidgetsMethod.Invoke(this.obj, new object[]
				{
					widgetRow
				});
			}
			Editable editable = this.obj as Editable;
			if (editable != null)
			{
				GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
				foreach (string text in editable.ConfigErrors())
				{
					listing.InfoText(text, this.nestDepth);
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x0013A7D8 File Offset: 0x001389D8
		public override string ToString()
		{
			string text = "EditTreeNode(";
			if (this.ParentObj != null)
			{
				string str = text;
				string str2 = " owningObj=";
				object parentObj = this.ParentObj;
				text = str + str2 + ((parentObj != null) ? parentObj.ToString() : null);
			}
			if (this.owningField != null)
			{
				string str3 = text;
				string str4 = " owningField=";
				FieldInfo fieldInfo = this.owningField;
				text = str3 + str4 + ((fieldInfo != null) ? fieldInfo.ToString() : null);
			}
			if (this.owningIndex >= 0)
			{
				text = text + " owningIndex=" + this.owningIndex.ToString();
			}
			return text + ")";
		}

		// Token: 0x04001F85 RID: 8069
		public object obj;

		// Token: 0x04001F86 RID: 8070
		public FieldInfo owningField;

		// Token: 0x04001F87 RID: 8071
		public int owningIndex = -1;

		// Token: 0x04001F88 RID: 8072
		private MethodInfo editWidgetsMethod;

		// Token: 0x04001F89 RID: 8073
		public EditTreeNodeType nodeType;

		// Token: 0x04001F8A RID: 8074
		private int indexToDelete = -1;
	}
}

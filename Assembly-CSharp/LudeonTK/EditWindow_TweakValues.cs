using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000058 RID: 88
	public class EditWindow_TweakValues : EditWindow
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00012CE7 File Offset: 0x00010EE7
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 600f);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00012CF8 File Offset: 0x00010EF8
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00012CFC File Offset: 0x00010EFC
		public EditWindow_TweakValues()
		{
			this.optionalTitle = "TweakValues";
			if (EditWindow_TweakValues.tweakValueFields == null)
			{
				EditWindow_TweakValues.tweakValueFields = (from field in this.FindAllTweakables()
				select new EditWindow_TweakValues.TweakInfo
				{
					field = field,
					tweakValue = field.TryGetAttribute<TweakValue>(),
					initial = this.GetAsFloat(field)
				} into ti
				orderby string.Format("{0}.{1}", ti.tweakValue.category, ti.field.DeclaringType.Name)
				select ti).ToList<EditWindow_TweakValues.TweakInfo>();
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00012D66 File Offset: 0x00010F66
		private IEnumerable<FieldInfo> FindAllTweakables()
		{
			return new EditWindow_TweakValues.<FindAllTweakables>d__11(-2);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00012D70 File Offset: 0x00010F70
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect;
			Rect outRect = rect = inRect.ContractedBy(4f);
			rect.xMax -= 33f;
			Rect rect2 = new Rect(0f, 0f, EditWindow_TweakValues.CategoryWidth, Text.CalcHeight("test", 1000f));
			Rect rect3 = new Rect(rect2.xMax, 0f, EditWindow_TweakValues.TitleWidth, rect2.height);
			Rect rect4 = new Rect(rect3.xMax, 0f, EditWindow_TweakValues.NumberWidth, rect2.height);
			Rect rect5 = new Rect(rect4.xMax, 0f, rect.width - rect4.xMax, rect2.height);
			DevGUI.BeginScrollView(outRect, ref this.scrollPosition, new Rect(0f, 0f, rect.width, rect2.height * (float)EditWindow_TweakValues.tweakValueFields.Count), true);
			foreach (EditWindow_TweakValues.TweakInfo tweakInfo in EditWindow_TweakValues.tweakValueFields)
			{
				DevGUI.Label(rect2, tweakInfo.tweakValue.category);
				DevGUI.Label(rect3, string.Format("{0}.{1}", tweakInfo.field.DeclaringType.Name, tweakInfo.field.Name));
				float num;
				bool flag;
				if (tweakInfo.field.FieldType == typeof(float) || tweakInfo.field.FieldType == typeof(int) || tweakInfo.field.FieldType == typeof(ushort))
				{
					float asFloat = this.GetAsFloat(tweakInfo.field);
					num = DevGUI.HorizontalSlider(rect5, this.GetAsFloat(tweakInfo.field), tweakInfo.tweakValue.min, tweakInfo.tweakValue.max);
					this.SetFromFloat(tweakInfo.field, num);
					flag = (asFloat != num);
				}
				else if (tweakInfo.field.FieldType == typeof(bool))
				{
					bool flag3;
					bool flag2 = flag3 = (bool)tweakInfo.field.GetValue(null);
					DevGUI.Checkbox(new Rect(rect5.xMin, rect5.yMin, rect2.height, rect2.height), ref flag3);
					tweakInfo.field.SetValue(null, flag3);
					num = (flag3 > false);
					flag = (flag2 != flag3);
				}
				else
				{
					Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944645);
					flag = false;
					num = tweakInfo.initial;
				}
				if (num != tweakInfo.initial)
				{
					GUI.color = Color.red;
					Text.WordWrap = false;
					DevGUI.Label(rect4, string.Format("{0} -> {1}", tweakInfo.initial, num));
					Text.WordWrap = true;
					GUI.color = Color.white;
					if (DevGUI.ButtonInvisible(rect4))
					{
						flag = true;
						if (tweakInfo.field.FieldType == typeof(float) || tweakInfo.field.FieldType == typeof(int) || tweakInfo.field.FieldType == typeof(ushort))
						{
							this.SetFromFloat(tweakInfo.field, tweakInfo.initial);
						}
						else if (tweakInfo.field.FieldType == typeof(bool))
						{
							tweakInfo.field.SetValue(null, tweakInfo.initial != 0f);
						}
						else
						{
							Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944646);
						}
					}
				}
				else
				{
					DevGUI.Label(rect4, string.Format("{0}", tweakInfo.initial));
				}
				if (flag)
				{
					MethodInfo method = tweakInfo.field.DeclaringType.GetMethod(tweakInfo.field.Name + "_Changed", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(null, null);
					}
				}
				rect2.y += rect2.height;
				rect3.y += rect2.height;
				rect4.y += rect2.height;
				rect5.y += rect2.height;
			}
			DevGUI.EndScrollView();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0001323C File Offset: 0x0001143C
		private float GetAsFloat(FieldInfo field)
		{
			if (field.FieldType == typeof(float))
			{
				return (float)field.GetValue(null);
			}
			if (field.FieldType == typeof(bool))
			{
				return (bool)field.GetValue(null) > false;
			}
			if (field.FieldType == typeof(int))
			{
				return (float)((int)field.GetValue(null));
			}
			if (field.FieldType == typeof(ushort))
			{
				return (float)((ushort)field.GetValue(null));
			}
			Log.ErrorOnce(string.Format("Attempted to return unknown field type {0} as a float", field.FieldType), 83944644);
			return 0f;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00013300 File Offset: 0x00011500
		private void SetFromFloat(FieldInfo field, float input)
		{
			if (field.FieldType == typeof(float))
			{
				field.SetValue(null, input);
				return;
			}
			if (field.FieldType == typeof(bool))
			{
				field.SetValue(null, input != 0f);
				return;
			}
			if (field.FieldType == typeof(int))
			{
				field.SetValue(field, (int)input);
				return;
			}
			if (field.FieldType == typeof(ushort))
			{
				field.SetValue(field, (ushort)input);
				return;
			}
			Log.ErrorOnce(string.Format("Attempted to set unknown field type {0} from a float", field.FieldType), 83944645);
		}

		// Token: 0x04000156 RID: 342
		[TweakValue("TweakValue", 0f, 300f)]
		public static float CategoryWidth = 180f;

		// Token: 0x04000157 RID: 343
		[TweakValue("TweakValue", 0f, 300f)]
		public static float TitleWidth = 300f;

		// Token: 0x04000158 RID: 344
		[TweakValue("TweakValue", 0f, 300f)]
		public static float NumberWidth = 140f;

		// Token: 0x04000159 RID: 345
		private Vector2 scrollPosition;

		// Token: 0x0400015A RID: 346
		private static List<EditWindow_TweakValues.TweakInfo> tweakValueFields;

		// Token: 0x0200247C RID: 9340
		private struct TweakInfo
		{
			// Token: 0x0400950A RID: 38154
			public FieldInfo field;

			// Token: 0x0400950B RID: 38155
			public TweakValue tweakValue;

			// Token: 0x0400950C RID: 38156
			public float initial;
		}
	}
}

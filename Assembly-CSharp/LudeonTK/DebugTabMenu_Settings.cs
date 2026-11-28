using System;
using System.Reflection;
using RimWorld;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200003F RID: 63
	public class DebugTabMenu_Settings : DebugTabMenu
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000BB33 File Offset: 0x00009D33
		public DebugTabMenu_Settings(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000BB40 File Offset: 0x00009D40
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Settings", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			foreach (FieldInfo fi in typeof(DebugSettings).GetFields())
			{
				this.AddNode(fi, "General");
			}
			foreach (FieldInfo fi2 in typeof(DebugViewSettings).GetFields())
			{
				this.AddNode(fi2, "View");
			}
			foreach (FieldInfo fi3 in typeof(DebugGenerationSettings).GetFields())
			{
				this.AddNode(fi3, "Generation");
			}
			return this.myRoot;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000BC00 File Offset: 0x00009E00
		private void AddNode(FieldInfo fi, string categoryLabel)
		{
			if (fi.IsLiteral)
			{
				return;
			}
			DebugActionNode debugActionNode = new DebugActionNode(this.LegibleFieldName(fi), DebugActionType.Action, delegate()
			{
				bool flag = (bool)fi.GetValue(null);
				fi.SetValue(null, !flag);
				MethodInfo method = fi.DeclaringType.GetMethod(fi.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}, null);
			debugActionNode.category = categoryLabel;
			debugActionNode.settingsField = fi;
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000BC67 File Offset: 0x00009E67
		private string LegibleFieldName(FieldInfo fi)
		{
			return GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
		}
	}
}

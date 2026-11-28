using System;
using System.Reflection;
using RimWorld;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200003E RID: 62
	public class DebugTabMenu_Output : DebugTabMenu
	{
		// Token: 0x06000195 RID: 405 RVA: 0x0000B9F4 File Offset: 0x00009BF4
		public DebugTabMenu_Output(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000BA00 File Offset: 0x00009C00
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Outputs", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugOutputAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
				}
			}
			this.myRoot.TrySort();
			return this.myRoot;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		private void GenerateCacheForMethod(MethodInfo method, DebugOutputAttribute attribute)
		{
			string label = attribute.name ?? GenText.SplitCamelCase(method.Name);
			Action action = Delegate.CreateDelegate(typeof(Action), method) as Action;
			DebugActionNode debugActionNode = new DebugActionNode(label, DebugActionType.Action, action, null);
			debugActionNode.category = (attribute.category ?? "General");
			debugActionNode.visibilityGetter = (() => !attribute.onlyWhenPlaying || Current.ProgramState == ProgramState.Playing);
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x040000B4 RID: 180
		public const string DefaultCategory = "General";
	}
}

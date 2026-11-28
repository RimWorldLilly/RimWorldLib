using System;
using System.Collections.Generic;
using LudeonTK;

namespace Verse
{
	// Token: 0x020005D8 RID: 1496
	public static class DebugTools_MapGen
	{
		// Token: 0x06002F89 RID: 12169 RVA: 0x00130F0C File Offset: 0x0012F10C
		public static List<DebugActionNode> Options_Scatterers()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localSt2 in typeof(GenStep_Scatterer).AllLeafSubclasses())
			{
				Type localSt = localSt2;
				list.Add(new DebugActionNode(localSt.ToString(), DebugActionType.ToolMap, null, null)
				{
					action = delegate()
					{
						((GenStep_Scatterer)Activator.CreateInstance(localSt)).ForceScatterAt(UI.MouseCell(), Find.CurrentMap);
					}
				});
			}
			return list;
		}
	}
}

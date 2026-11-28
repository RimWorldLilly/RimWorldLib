using System;
using RimWorld.Planet;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000034 RID: 52
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugActionAttribute : Attribute
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000AA74 File Offset: 0x00008C74
		public bool IsAllowedInCurrentGameState
		{
			get
			{
				bool flag = (this.allowedGameStates & AllowedGameStates.Entry) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Entry;
				bool flag2 = (this.allowedGameStates & AllowedGameStates.Playing) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Playing;
				bool flag3 = (this.allowedGameStates & AllowedGameStates.WorldRenderedNow) == AllowedGameStates.Invalid || WorldRendererUtility.WorldSelected;
				bool flag4 = (this.allowedGameStates & AllowedGameStates.IsCurrentlyOnMap) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldSelected && Find.CurrentMap != null);
				bool flag5 = (this.allowedGameStates & AllowedGameStates.HasGameCondition) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldSelected && Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ActiveConditions.Count > 0);
				bool flag6 = (!this.requiresRoyalty || ModsConfig.RoyaltyActive) && (!this.requiresIdeology || ModsConfig.IdeologyActive) && (!this.requiresBiotech || ModsConfig.BiotechActive) && (!this.requiresAnomaly || ModsConfig.AnomalyActive) && (!this.requiresOdyssey || ModsConfig.OdysseyActive);
				return flag && flag2 && flag3 && flag4 && flag5 && flag6;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000AB74 File Offset: 0x00008D74
		public DebugActionAttribute(string category = null, string name = null, bool requiresRoyalty = false, bool requiresIdeology = false, bool requiresBiotech = false, bool requiresAnomaly = false, bool requiresOdyssey = false, int displayPriority = 0, bool hideInSubMenu = false)
		{
			this.name = name;
			this.requiresRoyalty = requiresRoyalty;
			this.requiresIdeology = requiresIdeology;
			this.requiresBiotech = requiresBiotech;
			this.requiresAnomaly = requiresAnomaly;
			this.requiresOdyssey = requiresOdyssey;
			this.displayPriority = displayPriority;
			this.hideInSubMenu = hideInSubMenu;
			if (!string.IsNullOrEmpty(category))
			{
				this.category = category;
			}
		}

		// Token: 0x04000086 RID: 134
		public string name;

		// Token: 0x04000087 RID: 135
		public string category = "General";

		// Token: 0x04000088 RID: 136
		public AllowedGameStates allowedGameStates = AllowedGameStates.Playing;

		// Token: 0x04000089 RID: 137
		public DebugActionType actionType;

		// Token: 0x0400008A RID: 138
		public bool requiresRoyalty;

		// Token: 0x0400008B RID: 139
		public bool requiresIdeology;

		// Token: 0x0400008C RID: 140
		public bool requiresBiotech;

		// Token: 0x0400008D RID: 141
		public bool requiresAnomaly;

		// Token: 0x0400008E RID: 142
		public bool requiresOdyssey;

		// Token: 0x0400008F RID: 143
		public int displayPriority;

		// Token: 0x04000090 RID: 144
		public bool hideInSubMenu;
	}
}

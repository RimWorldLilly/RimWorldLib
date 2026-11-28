using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A68 RID: 6760
	public class Building_FirefoamPopper : Building
	{
		// Token: 0x17001DDE RID: 7646
		// (get) Token: 0x0600A9DB RID: 43483 RVA: 0x003C36BA File Offset: 0x003C18BA
		private bool CanSetAutoRebuild
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		// Token: 0x0600A9DC RID: 43484 RVA: 0x003C36E3 File Offset: 0x003C18E3
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.autoRebuild = (this.CanSetAutoRebuild && map.areaManager.Home[base.Position]);
			}
		}

		// Token: 0x0600A9DD RID: 43485 RVA: 0x003C3720 File Offset: 0x003C1920
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode != DestroyMode.Deconstruct && this.autoRebuild && map != null && this.CanSetAutoRebuild && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null, null, null, false, false, false).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff, null, null, true);
			}
		}

		// Token: 0x0600A9DE RID: 43486 RVA: 0x003C37A2 File Offset: 0x003C19A2
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_FirefoamPopper.<GetGizmos>d__6 <GetGizmos>d__ = new Building_FirefoamPopper.<GetGizmos>d__6(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A9DF RID: 43487 RVA: 0x003C37B2 File Offset: 0x003C19B2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRebuild, "autoRebuild", false, false);
		}

		// Token: 0x04006363 RID: 25443
		private bool autoRebuild;

		// Token: 0x04006364 RID: 25444
		private static readonly CachedTexture RebuildCommandTex = new CachedTexture("UI/Commands/RearmFirefoamPopper");
	}
}

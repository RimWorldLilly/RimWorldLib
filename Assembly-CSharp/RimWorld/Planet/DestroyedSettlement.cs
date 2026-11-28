using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023AC RID: 9132
	public class DestroyedSettlement : MapParent
	{
		// Token: 0x0600E359 RID: 58201 RVA: 0x0054920D File Offset: 0x0054740D
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.AnyBuildingBlockingMapRemoval)
			{
				return false;
			}
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x0600E35A RID: 58202 RVA: 0x00549248 File Offset: 0x00547448
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			DestroyedSettlement.<IncidentTargetTags>d__1 <IncidentTargetTags>d__ = new DestroyedSettlement.<IncidentTargetTags>d__1(-2);
			<IncidentTargetTags>d__.<>4__this = this;
			return <IncidentTargetTags>d__;
		}

		// Token: 0x0600E35B RID: 58203 RVA: 0x00549258 File Offset: 0x00547458
		public override IEnumerable<Gizmo> GetGizmos()
		{
			DestroyedSettlement.<GetGizmos>d__2 <GetGizmos>d__ = new DestroyedSettlement.<GetGizmos>d__2(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}
	}
}

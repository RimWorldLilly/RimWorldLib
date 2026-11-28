using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E1 RID: 4833
	public abstract class GenStep_Ambush : GenStep
	{
		// Token: 0x060077EE RID: 30702 RVA: 0x002A7010 File Offset: 0x002A5210
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rectToDefend;
			IntVec3 root;
			if (!SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out rectToDefend, out root, map))
			{
				return;
			}
			this.SpawnTrigger(rectToDefend, root, map, parms);
		}

		// Token: 0x060077EF RID: 30703 RVA: 0x002A7034 File Offset: 0x002A5234
		private void SpawnTrigger(CellRect rectToDefend, IntVec3 root, Map map, GenStepParams parms)
		{
			string signalTag = "ambushActivated-" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			CellRect rect;
			if (root.IsValid)
			{
				rect = CellRect.CenteredOn(root, 17);
			}
			else
			{
				rect = rectToDefend.ExpandedBy(12);
			}
			SignalAction_Ambush signalAction_Ambush = this.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.signalTag = signalTag;
			GenSpawn.Spawn(signalAction_Ambush, rect.CenterCell, map, WipeMode.Vanish);
			RectTrigger rectTrigger = this.MakeRectTrigger();
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
			triggerUnfogged.signalTag = signalTag;
			GenSpawn.Spawn(triggerUnfogged, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x060077F0 RID: 30704 RVA: 0x002A70E5 File Offset: 0x002A52E5
		protected virtual RectTrigger MakeRectTrigger()
		{
			return (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
		}

		// Token: 0x060077F1 RID: 30705 RVA: 0x002A70F8 File Offset: 0x002A52F8
		protected virtual SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			if (parms.sitePart != null)
			{
				signalAction_Ambush.points = parms.sitePart.parms.threatPoints;
			}
			else
			{
				signalAction_Ambush.points = this.defaultPointsRange.RandomInRange;
			}
			int num = Rand.RangeInclusive(0, 2);
			if (num == 0)
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Manhunters;
			}
			else if (num == 1 && PawnGroupMakerUtility.CanGenerateAnyNormalGroup(Faction.OfMechanoids, signalAction_Ambush.points))
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Mechanoids;
			}
			else
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Normal;
			}
			return signalAction_Ambush;
		}

		// Token: 0x04004B4B RID: 19275
		public FloatRange defaultPointsRange = new FloatRange(180f, 340f);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020019C7 RID: 6599
	public static class TransporterUtility
	{
		// Token: 0x0600A5C5 RID: 42437 RVA: 0x003B1A60 File Offset: 0x003AFC60
		public static void GetTransportersInGroup(int transportersGroup, Map map, List<CompTransporter> outTransporters)
		{
			outTransporters.Clear();
			if (transportersGroup < 0)
			{
				return;
			}
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Transporter);
			for (int i = 0; i < list.Count; i++)
			{
				CompTransporter compTransporter = list[i].TryGetComp<CompTransporter>();
				if (compTransporter.groupID == transportersGroup)
				{
					outTransporters.Add(compTransporter);
				}
			}
		}

		// Token: 0x0600A5C6 RID: 42438 RVA: 0x003B1AB4 File Offset: 0x003AFCB4
		public static Lord FindLord(int transportersGroup, Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters = lords[i].LordJob as LordJob_LoadAndEnterTransporters;
				if (lordJob_LoadAndEnterTransporters != null && lordJob_LoadAndEnterTransporters.transportersGroup == transportersGroup)
				{
					return lords[i];
				}
			}
			return null;
		}

		// Token: 0x0600A5C7 RID: 42439 RVA: 0x003B1B08 File Offset: 0x003AFD08
		public static bool WasLoadingCanceled(Thing transporter)
		{
			CompTransporter compTransporter = transporter.TryGetComp<CompTransporter>();
			return compTransporter != null && !compTransporter.LoadingInProgressOrReadyToLaunch;
		}

		// Token: 0x0600A5C8 RID: 42440 RVA: 0x003B1B2C File Offset: 0x003AFD2C
		public static int InitiateLoading(IEnumerable<CompTransporter> transporters)
		{
			int nextTransporterGroupID = Find.UniqueIDsManager.GetNextTransporterGroupID();
			foreach (CompTransporter compTransporter in transporters)
			{
				compTransporter.groupID = nextTransporterGroupID;
			}
			return nextTransporterGroupID;
		}

		// Token: 0x0600A5C9 RID: 42441 RVA: 0x003B1B80 File Offset: 0x003AFD80
		public static IEnumerable<Pawn> AllSendablePawns(List<CompTransporter> transporters, Map map)
		{
			TransporterUtility.<AllSendablePawns>d__4 <AllSendablePawns>d__ = new TransporterUtility.<AllSendablePawns>d__4(-2);
			<AllSendablePawns>d__.<>3__transporters = transporters;
			<AllSendablePawns>d__.<>3__map = map;
			return <AllSendablePawns>d__;
		}

		// Token: 0x0600A5CA RID: 42442 RVA: 0x003B1B97 File Offset: 0x003AFD97
		public static IEnumerable<Thing> AllSendableItems(List<CompTransporter> transporters, Map map)
		{
			TransporterUtility.<AllSendableItems>d__5 <AllSendableItems>d__ = new TransporterUtility.<AllSendableItems>d__5(-2);
			<AllSendableItems>d__.<>3__transporters = transporters;
			<AllSendableItems>d__.<>3__map = map;
			return <AllSendableItems>d__;
		}

		// Token: 0x0600A5CB RID: 42443 RVA: 0x003B1BAE File Offset: 0x003AFDAE
		public static IEnumerable<Thing> ThingsBeingHauledTo(List<CompTransporter> transporters, Map map)
		{
			TransporterUtility.<ThingsBeingHauledTo>d__6 <ThingsBeingHauledTo>d__ = new TransporterUtility.<ThingsBeingHauledTo>d__6(-2);
			<ThingsBeingHauledTo>d__.<>3__transporters = transporters;
			<ThingsBeingHauledTo>d__.<>3__map = map;
			return <ThingsBeingHauledTo>d__;
		}

		// Token: 0x0600A5CC RID: 42444 RVA: 0x003B1BC8 File Offset: 0x003AFDC8
		public static void MakeLordsAsAppropriate(List<Pawn> pawns, List<CompTransporter> transporters, Map map)
		{
			int groupID = transporters[0].groupID;
			Lord lord = null;
			IEnumerable<Pawn> enumerable = from x in pawns
			where (x.IsColonist || x.IsColonyMechPlayerControlled) && !x.Downed && x.Spawned
			select x;
			if (enumerable.Any<Pawn>())
			{
				lord = map.lordManager.lords.Find(delegate(Lord x)
				{
					LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters2 = x.LordJob as LordJob_LoadAndEnterTransporters;
					return lordJob_LoadAndEnterTransporters2 != null && lordJob_LoadAndEnterTransporters2.transportersGroup == groupID;
				});
				if (lord == null)
				{
					lord = LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_LoadAndEnterTransporters(groupID), map, null);
				}
				foreach (Pawn pawn in enumerable)
				{
					if (!lord.ownedPawns.Contains(pawn))
					{
						Lord lord2 = pawn.GetLord();
						if (lord2 != null)
						{
							lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
						}
						lord.AddPawn(pawn);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				for (int i = lord.ownedPawns.Count - 1; i >= 0; i--)
				{
					if (!enumerable.Contains(lord.ownedPawns[i]))
					{
						lord.Notify_PawnLost(lord.ownedPawns[i], PawnLostCondition.NoLongerEnteringTransportPods, null);
					}
				}
			}
			for (int j = map.lordManager.lords.Count - 1; j >= 0; j--)
			{
				LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters = map.lordManager.lords[j].LordJob as LordJob_LoadAndEnterTransporters;
				if (lordJob_LoadAndEnterTransporters != null && lordJob_LoadAndEnterTransporters.transportersGroup == groupID && map.lordManager.lords[j] != lord)
				{
					map.lordManager.RemoveLord(map.lordManager.lords[j]);
				}
			}
		}

		// Token: 0x0600A5CD RID: 42445 RVA: 0x003B1DA8 File Offset: 0x003AFFA8
		public static bool IncomingTransporterPreventingMapRemoval(Map map)
		{
			if (ModsConfig.OdysseyActive)
			{
				WorldComponent_GravshipController gravshipController = Find.GravshipController;
				if (gravshipController != null && gravshipController.LandingAreaConfirmationInProgress)
				{
					return true;
				}
				if (Find.CurrentGravship != null && Find.CurrentGravship.destinationTile == map.Tile)
				{
					return true;
				}
			}
			using (List<TravellingTransporters>.Enumerator enumerator = Find.WorldObjects.TravellingTransporters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.destinationTile == map.Tile)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

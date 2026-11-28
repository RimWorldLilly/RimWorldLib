using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200238C RID: 9100
	[StaticConstructorOnStartup]
	public static class CaravanMergeUtility
	{
		// Token: 0x170027D9 RID: 10201
		// (get) Token: 0x0600E256 RID: 57942 RVA: 0x0054057F File Offset: 0x0053E77F
		public static bool ShouldShowMergeCommand
		{
			get
			{
				return CaravanMergeUtility.CanMergeAnySelectedCaravans || CaravanMergeUtility.AnySelectedCaravanCloseToAnyOtherMergeableCaravan;
			}
		}

		// Token: 0x170027DA RID: 10202
		// (get) Token: 0x0600E257 RID: 57943 RVA: 0x00540590 File Offset: 0x0053E790
		public static bool CanMergeAnySelectedCaravans
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = i + 1; j < selectedObjects.Count; j++)
						{
							Caravan caravan2 = selectedObjects[j] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170027DB RID: 10203
		// (get) Token: 0x0600E258 RID: 57944 RVA: 0x0054060C File Offset: 0x0053E80C
		public static bool AnySelectedCaravanCloseToAnyOtherMergeableCaravan
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = 0; j < caravans.Count; j++)
						{
							Caravan caravan2 = caravans[j];
							if (caravan2 != caravan && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600E259 RID: 57945 RVA: 0x00540694 File Offset: 0x0053E894
		public static Command MergeCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandMergeCaravans".Translate();
			command_Action.defaultDesc = "CommandMergeCaravansDesc".Translate();
			command_Action.icon = CaravanMergeUtility.MergeCommandTex;
			command_Action.groupable = true;
			command_Action.action = delegate()
			{
				if (Find.WorldSelector.FirstSelectedObject != caravan)
				{
					return;
				}
				CaravanMergeUtility.TryMergeSelectedCaravans();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			};
			if (!CaravanMergeUtility.CanMergeAnySelectedCaravans)
			{
				command_Action.Disable("CommandMergeCaravansFailCaravansNotSelected".Translate());
			}
			return command_Action;
		}

		// Token: 0x0600E25A RID: 57946 RVA: 0x00540720 File Offset: 0x0053E920
		public static void TryMergeSelectedCaravans()
		{
			CaravanMergeUtility.tmpSelectedPlayerCaravans.Clear();
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				Caravan caravan3 = selectedObjects[i] as Caravan;
				if (caravan3 != null && caravan3.IsPlayerControlled)
				{
					CaravanMergeUtility.tmpSelectedPlayerCaravans.Add(caravan3);
				}
			}
			while (CaravanMergeUtility.tmpSelectedPlayerCaravans.Any<Caravan>())
			{
				Caravan caravan2 = CaravanMergeUtility.tmpSelectedPlayerCaravans[0];
				CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(0);
				CaravanMergeUtility.tmpCaravansOnSameTile.Clear();
				CaravanMergeUtility.tmpCaravansOnSameTile.Add(caravan2);
				CaravanMergeUtility.tmpShuttleCaravans.Clear();
				if (ModsConfig.OdysseyActive && caravan2.Shuttle != null)
				{
					CaravanMergeUtility.tmpShuttleCaravans.Add(caravan2);
				}
				for (int j = CaravanMergeUtility.tmpSelectedPlayerCaravans.Count - 1; j >= 0; j--)
				{
					if (CaravanMergeUtility.CloseToEachOther(CaravanMergeUtility.tmpSelectedPlayerCaravans[j], caravan2))
					{
						if (ModsConfig.OdysseyActive && CaravanMergeUtility.tmpSelectedPlayerCaravans[j].Shuttle != null)
						{
							CaravanMergeUtility.tmpShuttleCaravans.Add(CaravanMergeUtility.tmpSelectedPlayerCaravans[j]);
						}
						CaravanMergeUtility.tmpCaravansOnSameTile.Add(CaravanMergeUtility.tmpSelectedPlayerCaravans[j]);
						CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(j);
					}
				}
				if (CaravanMergeUtility.tmpCaravansOnSameTile.Count >= 2)
				{
					if (CaravanMergeUtility.tmpShuttleCaravans.Count >= 2)
					{
						string value = (from caravan in CaravanMergeUtility.tmpShuttleCaravans.Skip(1)
						select caravan.Shuttle.LabelCap).ToLineList("  - ", false);
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmMergeShuttles".Translate(value), delegate()
						{
							CaravanMergeUtility.MergeCaravans(CaravanMergeUtility.tmpCaravansOnSameTile);
						}, false, null, WindowLayer.Dialog));
					}
					else
					{
						CaravanMergeUtility.MergeCaravans(CaravanMergeUtility.tmpCaravansOnSameTile);
					}
				}
			}
		}

		// Token: 0x0600E25B RID: 57947 RVA: 0x00540908 File Offset: 0x0053EB08
		private static bool CloseToEachOther(Caravan c1, Caravan c2)
		{
			if (c1.Tile == c2.Tile)
			{
				return true;
			}
			Vector3 drawPos = c1.DrawPos;
			Vector3 drawPos2 = c2.DrawPos;
			float num = Find.WorldGrid.AverageTileSize * 0.5f;
			return (drawPos - drawPos2).sqrMagnitude < num * num;
		}

		// Token: 0x0600E25C RID: 57948 RVA: 0x00540960 File Offset: 0x0053EB60
		private static void MergeCaravans(List<Caravan> caravans)
		{
			bool flag = false;
			Caravan caravan = caravans.MaxBy((Caravan x) => x.PawnsListForReading.Count);
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (caravan2 != caravan)
				{
					if (ModsConfig.OdysseyActive && caravan2.Shuttle != null)
					{
						if (!flag)
						{
							flag = true;
						}
						else
						{
							Building_PassengerShuttle shuttle = caravan2.Shuttle;
							caravan2.GetDirectlyHeldThings().Remove(shuttle);
							caravan2.Shuttle.Destroy(DestroyMode.Vanish);
						}
					}
					caravan2.pawns.TryTransferAllToContainer(caravan.pawns, true);
					caravan2.Destroy();
				}
			}
			caravan.hasShuttleDirty = true;
			caravan.Notify_Merged(caravans);
		}

		// Token: 0x04009083 RID: 36995
		private static readonly Texture2D MergeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/MergeCaravans", true);

		// Token: 0x04009084 RID: 36996
		private static List<Caravan> tmpSelectedPlayerCaravans = new List<Caravan>();

		// Token: 0x04009085 RID: 36997
		private static List<Caravan> tmpCaravansOnSameTile = new List<Caravan>();

		// Token: 0x04009086 RID: 36998
		private static List<Caravan> tmpShuttleCaravans = new List<Caravan>();
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019D9 RID: 6617
	public class CompWakeUpDormant : ThingComp
	{
		// Token: 0x17001D1E RID: 7454
		// (get) Token: 0x0600A697 RID: 42647 RVA: 0x003B6160 File Offset: 0x003B4360
		private CompProperties_WakeUpDormant Props
		{
			get
			{
				return (CompProperties_WakeUpDormant)this.props;
			}
		}

		// Token: 0x17001D1F RID: 7455
		// (get) Token: 0x0600A698 RID: 42648 RVA: 0x003B6170 File Offset: 0x003B4370
		private CompCanBeDormant DormantComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormantComp) == null)
				{
					result = (this.dormantComp = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x0600A699 RID: 42649 RVA: 0x003B619B File Offset: 0x003B439B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpIfTargetClose = this.Props.wakeUpIfAnyTargetClose;
		}

		// Token: 0x0600A69A RID: 42650 RVA: 0x003B61B5 File Offset: 0x003B43B5
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x0600A69B RID: 42651 RVA: 0x003B61D0 File Offset: 0x003B43D0
		public void TickRareWorker()
		{
			if (!this.parent.Spawned || this.parent.Faction == Faction.OfPlayer)
			{
				return;
			}
			if (this.wakeUpIfTargetClose)
			{
				int num = GenRadial.NumCellsInRadius(this.Props.wakeUpCheckRadius);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map))
					{
						foreach (Thing thing in intVec.GetThingList(this.parent.Map))
						{
							if (this.Props.wakeUpTargetingParams.CanTarget(thing, null))
							{
								this.Activate(thing, true, false, false);
								return;
							}
						}
					}
				}
			}
			if (this.Props.wakeUpOnThingConstructedRadius > 0f)
			{
				Thing thing2 = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), this.Props.wakeUpOnThingConstructedRadius, (Thing t) => (t.def.building == null || t.def.building.wakeDormantPawnsOnConstruction) && t.Faction == Faction.OfPlayer, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing2 != null)
				{
					this.Activate(thing2, true, false, false);
				}
			}
		}

		// Token: 0x0600A69C RID: 42652 RVA: 0x003B6374 File Offset: 0x003B4574
		public void Activate(Thing waker, bool sendSignal = true, bool silent = false, bool instant = false)
		{
			if (this.DormantComp != null && this.DormantComp.Awake)
			{
				return;
			}
			if (sendSignal && (!this.sentSignal || !this.Props.onlySendSignalOnce))
			{
				if (!string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
				{
					if (this.Props.onlyWakeUpSameFaction)
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT"), this.parent.Faction.Named("FACTION")));
					}
					else
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT")));
					}
				}
				if (!silent && this.parent.Spawned && this.Props.wakeUpSound != null)
				{
					this.Props.wakeUpSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				if (this.parent.Spawned && waker is Pawn && !silent && (!this.Props.activateMessageKey.NullOrEmpty() || !this.Props.activatePluralMessageKey.NullOrEmpty()))
				{
					CompWakeUpDormant.tmpActivatedThings.Clear();
					if (this.groupID >= 0)
					{
						List<Thing> list = this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.WakeUpDormant);
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].TryGetComp<CompWakeUpDormant>().groupID == this.groupID)
							{
								CompWakeUpDormant.tmpActivatedThings.Add(list[i]);
							}
						}
					}
					else
					{
						CompWakeUpDormant.tmpActivatedThings.Add(this.parent);
					}
					if (CompWakeUpDormant.tmpActivatedThings.Count > 1 && !this.Props.activatePluralMessageKey.NullOrEmpty())
					{
						Messages.Message(this.Props.activatePluralMessageKey.Translate(waker.Named("WAKER")), CompWakeUpDormant.tmpActivatedThings, this.Props.activateMessageType ?? MessageTypeDefOf.NegativeEvent, false);
					}
					else if (CompWakeUpDormant.tmpActivatedThings.Count > 0 && !this.Props.activateMessageKey.NullOrEmpty())
					{
						Messages.Message(this.Props.activateMessageKey.Translate(waker.Named("WAKER")), CompWakeUpDormant.tmpActivatedThings, this.Props.activateMessageType ?? MessageTypeDefOf.NegativeEvent, false);
					}
					CompWakeUpDormant.tmpActivatedThings.Clear();
				}
				this.sentSignal = true;
			}
			if (this.DormantComp != null)
			{
				if (this.Props.wakeUpWithDelay && !instant)
				{
					this.DormantComp.WakeUpWithDelay();
					return;
				}
				this.DormantComp.WakeUp();
			}
		}

		// Token: 0x0600A69D RID: 42653 RVA: 0x003B6656 File Offset: 0x003B4856
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.wakeUpOnDamage && dinfo.Def.ExternalViolenceFor(this.parent))
			{
				this.Activate(dinfo.Instigator, true, false, true);
			}
		}

		// Token: 0x0600A69E RID: 42654 RVA: 0x003B668C File Offset: 0x003B488C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.wakeUpIfTargetClose && !this.Props.radiusCheckInspectPaneKey.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = text + (this.Props.radiusCheckInspectPaneKey.Translate() + ": ") + Mathf.RoundToInt(this.Props.wakeUpCheckRadius).ToString();
			}
			return text;
		}

		// Token: 0x0600A69F RID: 42655 RVA: 0x003B670D File Offset: 0x003B490D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.sentSignal, "sentSignal", false, false);
			Scribe_Values.Look<int>(ref this.groupID, "groupID", -1, false);
			Scribe_Values.Look<bool>(ref this.wakeUpIfTargetClose, "wakeUpIfColonistClose", false, false);
		}

		// Token: 0x04006235 RID: 25141
		public bool wakeUpIfTargetClose;

		// Token: 0x04006236 RID: 25142
		private bool sentSignal;

		// Token: 0x04006237 RID: 25143
		public int groupID = -1;

		// Token: 0x04006238 RID: 25144
		private CompCanBeDormant dormantComp;

		// Token: 0x04006239 RID: 25145
		private static List<Thing> tmpActivatedThings = new List<Thing>();
	}
}

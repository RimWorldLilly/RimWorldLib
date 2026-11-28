using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001ADC RID: 6876
	[StaticConstructorOnStartup]
	public class Building_MechGestator : Building_WorkTableAutonomous
	{
		// Token: 0x17001FA0 RID: 8096
		// (get) Token: 0x0600AF75 RID: 44917 RVA: 0x003DC500 File Offset: 0x003DA700
		public Bill_Mech ActiveMechBill
		{
			get
			{
				return base.ActiveBill as Bill_Mech;
			}
		}

		// Token: 0x17001FA1 RID: 8097
		// (get) Token: 0x0600AF76 RID: 44918 RVA: 0x003DC50D File Offset: 0x003DA70D
		public CompWasteProducer WasteProducer
		{
			get
			{
				if (this.wasteProducer == null)
				{
					this.wasteProducer = base.GetComp<CompWasteProducer>();
				}
				return this.wasteProducer;
			}
		}

		// Token: 0x17001FA2 RID: 8098
		// (get) Token: 0x0600AF77 RID: 44919 RVA: 0x003DC529 File Offset: 0x003DA729
		public CompPowerTrader Power
		{
			get
			{
				if (this.power == null)
				{
					this.power = this.TryGetComp<CompPowerTrader>();
				}
				return this.power;
			}
		}

		// Token: 0x17001FA3 RID: 8099
		// (get) Token: 0x0600AF78 RID: 44920 RVA: 0x003DC545 File Offset: 0x003DA745
		public bool PoweredOn
		{
			get
			{
				return this.Power.PowerOn;
			}
		}

		// Token: 0x17001FA4 RID: 8100
		// (get) Token: 0x0600AF79 RID: 44921 RVA: 0x003DC552 File Offset: 0x003DA752
		public bool BoundPawnStateAllowsForming
		{
			get
			{
				return this.ActiveMechBill.BoundPawn != null && !this.ActiveMechBill.BoundPawn.Dead && !this.ActiveMechBill.BoundPawn.Suspended;
			}
		}

		// Token: 0x17001FA5 RID: 8101
		// (get) Token: 0x0600AF7A RID: 44922 RVA: 0x003DC588 File Offset: 0x003DA788
		public Pawn GestatingMech
		{
			get
			{
				Pawn pawn = (Pawn)this.innerContainer.FirstOrDefault((Thing t) => t is Pawn);
				if (pawn != null)
				{
					return pawn;
				}
				Corpse resurrectingMechCorpse = this.ResurrectingMechCorpse;
				if (resurrectingMechCorpse != null)
				{
					return resurrectingMechCorpse.InnerPawn;
				}
				return null;
			}
		}

		// Token: 0x17001FA6 RID: 8102
		// (get) Token: 0x0600AF7B RID: 44923 RVA: 0x003DC5DC File Offset: 0x003DA7DC
		public Corpse ResurrectingMechCorpse
		{
			get
			{
				return (Corpse)this.innerContainer.FirstOrDefault((Thing t) => t is Corpse);
			}
		}

		// Token: 0x0600AF7C RID: 44924 RVA: 0x003DC60D File Offset: 0x003DA80D
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Mech gestator"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600AF7D RID: 44925 RVA: 0x003DC629 File Offset: 0x003DA829
		public bool CanBeUsedNowBy(Pawn user)
		{
			return this.ActiveMechBill == null || this.ActiveMechBill.BoundPawn == user;
		}

		// Token: 0x0600AF7E RID: 44926 RVA: 0x003DC643 File Offset: 0x003DA843
		public override void Notify_StartForming(Pawn billDoer)
		{
			SoundDefOf.MechGestatorCycle_Started.PlayOneShot(this);
		}

		// Token: 0x0600AF7F RID: 44927 RVA: 0x003DC658 File Offset: 0x003DA858
		public override void Notify_FormingCompleted()
		{
			Pawn pawn = this.activeBill.CreateProducts() as Pawn;
			Messages.Message("GestationComplete".Translate() + ": " + pawn.kindDef.LabelCap, this, MessageTypeDefOf.PositiveEvent, true);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			this.innerContainer.TryAdd(pawn, true);
			this.WasteProducer.ProduceWaste((int)pawn.GetStatValue(StatDefOf.WastepacksPerRecharge, true, -1));
			SoundDefOf.MechGestatorBill_Completed.PlayOneShot(this);
		}

		// Token: 0x0600AF80 RID: 44928 RVA: 0x003DC6F3 File Offset: 0x003DA8F3
		public override void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			SoundDefOf.MechGestator_MaterialInserted.PlayOneShot(this);
		}

		// Token: 0x0600AF81 RID: 44929 RVA: 0x003DC708 File Offset: 0x003DA908
		public override void EjectContents()
		{
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.innerContainer[i] as Pawn;
				if (pawn != null)
				{
					this.innerContainer.RemoveAt(i);
					pawn.Destroy(DestroyMode.Vanish);
				}
			}
			base.EjectContents();
		}

		// Token: 0x0600AF82 RID: 44930 RVA: 0x003DC75C File Offset: 0x003DA95C
		protected override void Tick()
		{
			base.Tick();
			if (this.activeBill != null && this.PoweredOn && this.BoundPawnStateAllowsForming)
			{
				this.activeBill.BillTick();
				ThingDef thingDef = null;
				if (this.ActiveMechBill.State == FormingState.Forming)
				{
					thingDef = this.def.building.gestatorFormingMote.GetForRotation(base.Rotation);
				}
				else if (this.ActiveMechBill.State == FormingState.Preparing && this.ActiveMechBill.GestationCyclesCompleted > 0)
				{
					thingDef = this.def.building.gestatorCycleCompleteMote.GetForRotation(base.Rotation);
				}
				else if (this.ActiveMechBill.State == FormingState.Formed)
				{
					thingDef = this.def.building.gestatorFormedMote.GetForRotation(base.Rotation);
				}
				if (thingDef != null)
				{
					if (this.workingMote == null || this.workingMote.Destroyed || this.workingMote.def != thingDef)
					{
						this.workingMote = MoteMaker.MakeAttachedOverlay(this, thingDef, Vector3.zero, 1f, -1f);
					}
					Mote mote = this.workingMote;
					if (mote != null)
					{
						mote.Maintain();
					}
				}
			}
			if (this.IsHashIntervalTick(250))
			{
				if (this.activeBill != null && this.activeBill.State == FormingState.Forming)
				{
					this.Power.PowerOutput = -this.Power.Props.PowerConsumption;
				}
				else
				{
					this.Power.PowerOutput = -this.Power.Props.idlePowerDraw;
				}
			}
			if (this.activeBill != null && this.PoweredOn && this.activeBill.State != FormingState.Gathering)
			{
				if (this.workingSound == null || this.workingSound.Ended)
				{
					this.workingSound = SoundDefOf.MechGestator_Ambience.TrySpawnSustainer(this);
				}
				this.workingSound.Maintain();
				return;
			}
			if (this.workingSound != null)
			{
				this.workingSound.End();
				this.workingSound = null;
			}
		}

		// Token: 0x0600AF83 RID: 44931 RVA: 0x003DC94C File Offset: 0x003DAB4C
		protected override string GetInspectStringExtra()
		{
			Bill_Mech bill_Mech = this.activeBill as Bill_Mech;
			if (bill_Mech == null || bill_Mech.State != FormingState.Forming)
			{
				return null;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("GestatingInspect".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(Mathf.CeilToInt(bill_Mech.formingTicks * 1f / bill_Mech.WorkSpeedMultiplier).ToStringTicksToPeriod(true, false, true, true, false));
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600AF84 RID: 44932 RVA: 0x003DC9CC File Offset: 0x003DABCC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.activeBill != null && this.activeBill.State != FormingState.Gathering && this.def.building.formingGraphicData != null)
			{
				Vector3 loc = drawLoc + this.def.building.formingMechPerRotationOffset[base.Rotation.AsInt];
				loc.y += 0.018292684f;
				loc.z += Mathf.PingPong((float)Find.TickManager.TicksGame * this.def.building.formingMechBobSpeed, this.def.building.formingMechYBobDistance);
				Graphic graphic;
				if (this.TryGetMechFormingGraphic(out graphic))
				{
					graphic.Draw(loc, Rot4.South, this, 0f);
				}
				else
				{
					this.def.building.formingGraphicData.Graphic.Draw(loc, Rot4.North, this, 0f);
				}
			}
			GenDraw.FillableBarRequest barDrawData = base.BarDrawData;
			barDrawData.center = drawLoc;
			barDrawData.fillPercent = base.CurrentBillFormingPercent;
			barDrawData.filledMat = Building_MechGestator.FormingCycleBarFilledMat;
			barDrawData.unfilledMat = Building_MechGestator.FormingCycleUnfilledMat;
			barDrawData.rotation = base.Rotation;
			GenDraw.DrawFillableBar(barDrawData);
			if (this.topGraphic == null)
			{
				this.topGraphic = this.def.building.mechGestatorTopGraphic.GraphicColoredFor(this);
			}
			if (this.cylinderGraphic == null)
			{
				this.cylinderGraphic = this.def.building.mechGestatorCylinderGraphic.GraphicColoredFor(this);
			}
			Vector3 loc2 = new Vector3(drawLoc.x, AltitudeLayer.BuildingBelowTop.AltitudeFor(), drawLoc.z);
			this.cylinderGraphic.Draw(loc2, base.Rotation, this, 0f);
			Vector3 loc3 = new Vector3(drawLoc.x, AltitudeLayer.BuildingOnTop.AltitudeFor(), drawLoc.z);
			this.topGraphic.Draw(loc3, base.Rotation, this, 0f);
		}

		// Token: 0x0600AF85 RID: 44933 RVA: 0x003DCBC0 File Offset: 0x003DADC0
		private bool TryGetMechFormingGraphic(out Graphic graphic)
		{
			graphic = null;
			if (this.ResurrectingMechCorpse != null)
			{
				graphic = this.ResurrectingMechCorpse.InnerPawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
			}
			else if (this.GestatingMech != null)
			{
				graphic = this.GestatingMech.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
			}
			if (graphic != null && graphic.drawSize.x <= this.def.building.maxFormedMechDrawSize.x && graphic.drawSize.y <= this.def.building.maxFormedMechDrawSize.y)
			{
				return true;
			}
			graphic = null;
			return false;
		}

		// Token: 0x0600AF86 RID: 44934 RVA: 0x003DCC6F File Offset: 0x003DAE6F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_MechGestator.<GetGizmos>d__32 <GetGizmos>d__ = new Building_MechGestator.<GetGizmos>d__32(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AF87 RID: 44935 RVA: 0x003DCC80 File Offset: 0x003DAE80
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Bill_ResurrectMech bill_ResurrectMech = this.ActiveMechBill as Bill_ResurrectMech;
				if (bill_ResurrectMech != null && bill_ResurrectMech.State != FormingState.Gathering && this.GestatingMech == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Bill ");
					defaultInterpolatedStringHandler.AppendFormatted<Bill_Autonomous>(base.ActiveBill);
					defaultInterpolatedStringHandler.AppendLiteral(" with recipe ");
					defaultInterpolatedStringHandler.AppendFormatted<RecipeDef>(base.ActiveBill.recipe);
					defaultInterpolatedStringHandler.AppendLiteral(" loaded with no mech to resurrect. Resetting bill.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					base.ActiveBill.Reset();
				}
			}
		}

		// Token: 0x04006552 RID: 25938
		private Mote workingMote;

		// Token: 0x04006553 RID: 25939
		private Sustainer workingSound;

		// Token: 0x04006554 RID: 25940
		private Graphic cylinderGraphic;

		// Token: 0x04006555 RID: 25941
		private Graphic topGraphic;

		// Token: 0x04006556 RID: 25942
		private CompPowerTrader power;

		// Token: 0x04006557 RID: 25943
		private CompWasteProducer wasteProducer;

		// Token: 0x04006558 RID: 25944
		private static Material FormingCycleBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.98f, 0.46f, 0f), false);

		// Token: 0x04006559 RID: 25945
		private static Material FormingCycleUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 0f, 0f), false);
	}
}

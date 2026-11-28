using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200146B RID: 5227
	public abstract class CompAbilityEffect_WithDest : CompAbilityEffect, ITargetingSource
	{
		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x06008047 RID: 32839 RVA: 0x002DC36C File Offset: 0x002DA56C
		public new CompProperties_EffectWithDest Props
		{
			get
			{
				return (CompProperties_EffectWithDest)this.props;
			}
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x06008048 RID: 32840 RVA: 0x002DC379 File Offset: 0x002DA579
		public virtual TargetingParameters targetParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetLocations = true
				};
			}
		}

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x06008049 RID: 32841 RVA: 0x002DC387 File Offset: 0x002DA587
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x0600804A RID: 32842 RVA: 0x002DC38A File Offset: 0x002DA58A
		public Thing Caster
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x0600804B RID: 32843 RVA: 0x002DC397 File Offset: 0x002DA597
		public Pawn CasterPawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x0600804C RID: 32844 RVA: 0x002DC3A4 File Offset: 0x002DA5A4
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x0600804D RID: 32845 RVA: 0x002DC3A7 File Offset: 0x002DA5A7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x0600804E RID: 32846 RVA: 0x002DC3AA File Offset: 0x002DA5AA
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x0600804F RID: 32847 RVA: 0x002DC3AD File Offset: 0x002DA5AD
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x06008050 RID: 32848 RVA: 0x002DC3B0 File Offset: 0x002DA5B0
		public Texture2D UIIcon
		{
			get
			{
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x06008051 RID: 32849 RVA: 0x002DC3B7 File Offset: 0x002DA5B7
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x06008052 RID: 32850 RVA: 0x002DC3BA File Offset: 0x002DA5BA
		public bool HidePawnTooltips
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008053 RID: 32851 RVA: 0x002DC3C0 File Offset: 0x002DA5C0
		public LocalTargetInfo GetDestination(LocalTargetInfo target)
		{
			Map map = this.parent.pawn.Map;
			switch (this.Props.destination)
			{
			case AbilityEffectDestination.Caster:
				return new LocalTargetInfo(this.parent.pawn.InteractionCell);
			case AbilityEffectDestination.RandomInRange:
			{
				this.cells.Clear();
				int num = GenRadial.NumCellsInRadius(this.Props.randomRange.max);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = GenRadial.RadialPattern[i];
					if (intVec.DistanceTo(IntVec3.Zero) >= this.Props.randomRange.min)
					{
						IntVec3 intVec2 = target.Cell + intVec;
						if (intVec2.Standable(map) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(target.Cell, intVec2, map)) && !intVec2.Fogged(map))
						{
							this.cells.Add(intVec2);
						}
					}
				}
				if (this.cells.Any<IntVec3>())
				{
					return new LocalTargetInfo(this.cells.RandomElement<IntVec3>());
				}
				Messages.Message("NoValidDestinationFound".Translate(this.parent.def.LabelCap), MessageTypeDefOf.RejectInput, true);
				return LocalTargetInfo.Invalid;
			}
			case AbilityEffectDestination.Selected:
				return target;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06008054 RID: 32852 RVA: 0x002DC51C File Offset: 0x002DA71C
		public bool CanPlaceSelectedTargetAt(LocalTargetInfo target)
		{
			Pawn pawn = this.selectedTarget.Pawn;
			if (pawn != null)
			{
				Building_Door door = target.Cell.GetDoor(pawn.MapHeld);
				return (door == null || door.CanPhysicallyPass(pawn)) && (pawn.Spawned && !target.Cell.Impassable(this.parent.pawn.Map)) && target.Cell.WalkableBy(this.parent.pawn.Map, pawn);
			}
			return CompAbilityEffect_WithDest.CanTeleportThingTo(target, this.parent.pawn.Map);
		}

		// Token: 0x06008055 RID: 32853 RVA: 0x002DC5B8 File Offset: 0x002DA7B8
		public static bool CanTeleportThingTo(LocalTargetInfo target, Map map)
		{
			Building edifice = target.Cell.GetEdifice(map);
			if (edifice != null && edifice.def.surfaceType != SurfaceType.Item && edifice.def.surfaceType != SurfaceType.Eat)
			{
				Building_Door building_Door = edifice as Building_Door;
				if (building_Door == null || !building_Door.Open)
				{
					return false;
				}
			}
			List<Thing> thingList = target.Cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Item)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008056 RID: 32854 RVA: 0x002DC63C File Offset: 0x002DA83C
		public virtual bool CanHitTarget(LocalTargetInfo target)
		{
			return target.IsValid && (this.Props.range <= 0f || target.Cell.DistanceTo(this.selectedTarget.Cell) <= this.Props.range) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(this.selectedTarget.Cell, target.Cell, this.parent.pawn.Map));
		}

		// Token: 0x06008057 RID: 32855 RVA: 0x002DC6C5 File Offset: 0x002DA8C5
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.CanHitTarget(target);
		}

		// Token: 0x06008058 RID: 32856 RVA: 0x002DC6D0 File Offset: 0x002DA8D0
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (this.Props.range > 0f)
			{
				if (this.Props.requiresLineOfSight)
				{
					GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.selectedTarget.Cell, c, this.parent.pawn.Map) && this.CanPlaceSelectedTargetAt(c));
				}
				else
				{
					GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range);
				}
			}
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06008059 RID: 32857 RVA: 0x002DC754 File Offset: 0x002DA954
		public void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				icon = this.parent.def.uiIcon;
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
			string text = this.ExtraLabelMouseAttachment(target);
			if (!text.NullOrEmpty())
			{
				Widgets.MouseAttachedLabel(text, 0f, 0f, null);
			}
		}

		// Token: 0x0600805A RID: 32858 RVA: 0x002DC7B2 File Offset: 0x002DA9B2
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.parent.QueueCastingJob(this.selectedTarget, target);
			this.selectedTarget = LocalTargetInfo.Invalid;
		}

		// Token: 0x0600805B RID: 32859 RVA: 0x002DC7D1 File Offset: 0x002DA9D1
		public void SetTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target;
		}

		// Token: 0x0600805C RID: 32860 RVA: 0x002DC7DA File Offset: 0x002DA9DA
		public virtual void SelectDestination()
		{
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x0600805D RID: 32861 RVA: 0x002DC7EC File Offset: 0x002DA9EC
		public bool SelectedTargetInvalidated()
		{
			return this.selectedTarget.HasThing && !this.selectedTarget.Thing.Spawned;
		}

		// Token: 0x04005019 RID: 20505
		protected LocalTargetInfo selectedTarget = LocalTargetInfo.Invalid;

		// Token: 0x0400501A RID: 20506
		private List<IntVec3> cells = new List<IntVec3>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020019D1 RID: 6609
	public class CompUsable : ThingComp, ITargetingSource
	{
		// Token: 0x17001D0D RID: 7437
		// (get) Token: 0x0600A64B RID: 42571 RVA: 0x003B4F1A File Offset: 0x003B311A
		public CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)this.props;
			}
		}

		// Token: 0x17001D0E RID: 7438
		// (get) Token: 0x0600A64C RID: 42572 RVA: 0x003B4F28 File Offset: 0x003B3128
		private Texture2D Icon
		{
			get
			{
				if (this.icon == null && this.Props.floatMenuFactionIcon != null)
				{
					Faction faction = Find.FactionManager.FirstFactionOfDef(this.Props.floatMenuFactionIcon);
					Texture2D texture2D;
					if (faction == null)
					{
						texture2D = null;
					}
					else
					{
						FactionDef def = faction.def;
						texture2D = ((def != null) ? def.FactionIcon : null);
					}
					this.icon = texture2D;
				}
				return this.icon;
			}
		}

		// Token: 0x17001D0F RID: 7439
		// (get) Token: 0x0600A64D RID: 42573 RVA: 0x003B4F8C File Offset: 0x003B318C
		private Color IconColor
		{
			get
			{
				if (this.iconColor == null && this.Props.floatMenuFactionIcon != null)
				{
					Faction faction = Find.FactionManager.FirstFactionOfDef(this.Props.floatMenuFactionIcon);
					this.iconColor = ((faction != null) ? new Color?(faction.Color) : null);
				}
				Color? color = this.iconColor;
				if (color == null)
				{
					return Color.white;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x17001D10 RID: 7440
		// (get) Token: 0x0600A64E RID: 42574 RVA: 0x003B5004 File Offset: 0x003B3204
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D11 RID: 7441
		// (get) Token: 0x0600A64F RID: 42575 RVA: 0x003B5007 File Offset: 0x003B3207
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D12 RID: 7442
		// (get) Token: 0x0600A650 RID: 42576 RVA: 0x003B500A File Offset: 0x003B320A
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D13 RID: 7443
		// (get) Token: 0x0600A651 RID: 42577 RVA: 0x003B500D File Offset: 0x003B320D
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D14 RID: 7444
		// (get) Token: 0x0600A652 RID: 42578 RVA: 0x003B5010 File Offset: 0x003B3210
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D15 RID: 7445
		// (get) Token: 0x0600A653 RID: 42579 RVA: 0x003B5013 File Offset: 0x003B3213
		public Thing Caster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001D16 RID: 7446
		// (get) Token: 0x0600A654 RID: 42580 RVA: 0x003B501B File Offset: 0x003B321B
		public Pawn CasterPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001D17 RID: 7447
		// (get) Token: 0x0600A655 RID: 42581 RVA: 0x003B501E File Offset: 0x003B321E
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001D18 RID: 7448
		// (get) Token: 0x0600A656 RID: 42582 RVA: 0x003B5021 File Offset: 0x003B3221
		public TargetingParameters targetParams
		{
			get
			{
				return TargetingParameters.ForPawns();
			}
		}

		// Token: 0x17001D19 RID: 7449
		// (get) Token: 0x0600A657 RID: 42583 RVA: 0x003B5028 File Offset: 0x003B3228
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001D1A RID: 7450
		// (get) Token: 0x0600A658 RID: 42584 RVA: 0x003B502B File Offset: 0x003B322B
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600A659 RID: 42585 RVA: 0x003B502E File Offset: 0x003B322E
		protected virtual string FloatMenuOptionLabel(Pawn pawn)
		{
			return this.Props.useLabel.Formatted(this.parent);
		}

		// Token: 0x0600A65A RID: 42586 RVA: 0x003B5050 File Offset: 0x003B3250
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			CompUsable.<CompFloatMenuOptions>d__31 <CompFloatMenuOptions>d__ = new CompUsable.<CompFloatMenuOptions>d__31(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__myPawn = myPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A65B RID: 42587 RVA: 0x003B5067 File Offset: 0x003B3267
		public virtual LocalTargetInfo GetExtraTarget(Pawn pawn)
		{
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x0600A65C RID: 42588 RVA: 0x003B5070 File Offset: 0x003B3270
		public virtual void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget, bool forced = false)
		{
			CompUsable.<>c__DisplayClass33_0 CS$<>8__locals1 = new CompUsable.<>c__DisplayClass33_0();
			CS$<>8__locals1.extraTarget = extraTarget;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.<>4__this = this;
			if (this.Props.useJob == null)
			{
				return;
			}
			if (!this.CanBeUsedBy(CS$<>8__locals1.pawn, forced, false))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CompUseEffect compUseEffect in this.parent.GetComps<CompUseEffect>())
			{
				TaggedString taggedString = compUseEffect.ConfirmMessage(CS$<>8__locals1.pawn);
				if (!taggedString.NullOrEmpty())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendTagged(taggedString);
				}
			}
			string str = stringBuilder.ToString();
			if (str.NullOrEmpty())
			{
				CS$<>8__locals1.<TryStartUseJob>g__StartJob|0();
				return;
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, new Action(CS$<>8__locals1.<TryStartUseJob>g__StartJob|0), false, null, WindowLayer.Dialog));
		}

		// Token: 0x0600A65D RID: 42589 RVA: 0x003B5168 File Offset: 0x003B3368
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompUsable.<CompGetGizmosExtra>d__34 <CompGetGizmosExtra>d__ = new CompUsable.<CompGetGizmosExtra>d__34(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A65E RID: 42590 RVA: 0x003B5178 File Offset: 0x003B3378
		public virtual void UsedBy(Pawn p)
		{
			if (!this.CanBeUsedBy(p, false, true))
			{
				return;
			}
			foreach (CompUseEffect compUseEffect in from x in this.parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
			{
				try
				{
					compUseEffect.DoEffect(p);
				}
				catch (Exception ex)
				{
					string str = "Error in CompUseEffect: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (this.Props.useMessage != null)
			{
				Messages.Message(this.Props.useMessage.Formatted(p.Named("PAWN"), this.parent), this.parent, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x0600A65F RID: 42591 RVA: 0x003B527C File Offset: 0x003B347C
		public virtual AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return false;
			}
			if (p.IsMutant && !this.Props.allowedMutants.Contains(p.mutant.Def))
			{
				return false;
			}
			PlanetTile tile = p.MapHeld.Tile;
			if (tile.Valid && !this.Props.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.Props.layerWhitelist.Contains(tile.LayerDef))
			{
				return "CannotPerformPlanetLayer".Translate(tile.LayerDef.gerundLabel.Named("GERUND"), tile.LayerDef.label.Named("LAYER")).Resolve();
			}
			if (tile.Valid && !this.Props.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && this.Props.layerBlacklist.Contains(tile.LayerDef))
			{
				return "CannotPerformPlanetLayer".Translate(tile.LayerDef.gerundLabel.Named("GERUND"), tile.LayerDef.label.Named("LAYER")).Resolve();
			}
			CompPowerTrader compPowerTrader;
			if (this.parent.TryGetComp(out compPowerTrader) && !compPowerTrader.PowerOn)
			{
				return "NoPower".Translate();
			}
			if (!ignoreReserveAndReachable && !p.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (!ignoreReserveAndReachable && !p.CanReserve(this.parent, 1, -1, null, forced))
			{
				Pawn pawn = p.Map.reservationManager.FirstRespectedReserver(this.parent, p, null) ?? p.Map.physicalInteractionReservationManager.FirstReserverOf(this.parent);
				if (pawn != null)
				{
					return "ReservedBy".Translate(pawn.LabelShort, pawn);
				}
				return "Reserved".Translate();
			}
			else
			{
				if (this.Props.userMustHaveHediff != null && !p.health.hediffSet.HasHediff(this.Props.userMustHaveHediff, false))
				{
					return "MustHaveHediff".Translate(this.Props.userMustHaveHediff);
				}
				List<ThingComp> allComps = this.parent.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					CompUseEffect compUseEffect = allComps[i] as CompUseEffect;
					if (compUseEffect != null)
					{
						AcceptanceReport result = compUseEffect.CanBeUsedBy(p);
						if (!result.Accepted)
						{
							return result;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600A660 RID: 42592 RVA: 0x003B5544 File Offset: 0x003B3744
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x0600A661 RID: 42593 RVA: 0x003B5550 File Offset: 0x003B3750
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn;
			if (!target.IsValid || !target.TryGetPawn(out pawn) || !pawn.IsPlayerControlled)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = this.CanBeUsedBy(pawn, this.Props.ignoreOtherReservations, false);
			if (!acceptanceReport.Accepted)
			{
				if (showMessages && !acceptanceReport.Reason.NullOrEmpty())
				{
					this.SendCannotUseMessage(pawn, acceptanceReport.Reason);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600A662 RID: 42594 RVA: 0x003B55BC File Offset: 0x003B37BC
		public void SendCannotUseMessage(Pawn pawn, string reason)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CannotGenericWorkCustom".Translate(this.FloatMenuOptionLabel(pawn)));
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(reason.CapitalizeFirst());
			Messages.Message(defaultInterpolatedStringHandler.ToStringAndClear(), pawn, MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0600A663 RID: 42595 RVA: 0x003B5620 File Offset: 0x003B3820
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600A664 RID: 42596 RVA: 0x003B5634 File Offset: 0x003B3834
		public void OrderForceTarget(LocalTargetInfo target)
		{
			using (IEnumerator<CompUseEffect> enumerator = this.parent.GetComps<CompUseEffect>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SelectedUseOption(target.Pawn))
					{
						return;
					}
				}
			}
			this.TryStartUseJob(target.Pawn, this.GetExtraTarget(target.Pawn), this.Props.ignoreOtherReservations);
		}

		// Token: 0x0600A665 RID: 42597 RVA: 0x003B56B4 File Offset: 0x003B38B4
		public void OnGUI(LocalTargetInfo target)
		{
			Widgets.MouseAttachedLabel("UseGizmoMouse".Translate(), 0f, 0f, null);
			if (this.ValidateTarget(target, false))
			{
				GenUI.DrawMouseAttachment(TexCommand.Attack);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x04006219 RID: 25113
		private Texture2D icon;

		// Token: 0x0400621A RID: 25114
		private Color? iconColor;
	}
}

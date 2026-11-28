using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001357 RID: 4951
	public class CompHibernatable : ThingComp
	{
		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x06007A96 RID: 31382 RVA: 0x002BA95C File Offset: 0x002B8B5C
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x06007A97 RID: 31383 RVA: 0x002BA969 File Offset: 0x002B8B69
		// (set) Token: 0x06007A98 RID: 31384 RVA: 0x002BA971 File Offset: 0x002B8B71
		public HibernatableStateDef State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state == value)
				{
					return;
				}
				this.state = value;
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x06007A99 RID: 31385 RVA: 0x002BA99E File Offset: 0x002B8B9E
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x06007A9A RID: 31386 RVA: 0x002BA9AD File Offset: 0x002B8BAD
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x06007A9B RID: 31387 RVA: 0x002BA9D3 File Offset: 0x002B8BD3
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			map.info.parent.Notify_HibernatableChanged();
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06007A9C RID: 31388 RVA: 0x002BAA10 File Offset: 0x002B8C10
		public void Startup()
		{
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				Log.ErrorOnce("Attempted to start a non-hibernating object", 34361223);
				return;
			}
			this.State = HibernatableStateDefOf.Starting;
			this.endStartupTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + this.Props.startupDays * 60000f);
		}

		// Token: 0x06007A9D RID: 31389 RVA: 0x002BAA70 File Offset: 0x002B8C70
		public override string CompInspectStringExtra()
		{
			if (this.State == HibernatableStateDefOf.Hibernating)
			{
				return "HibernatableHibernating".Translate();
			}
			if (this.State == HibernatableStateDefOf.Starting)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("HibernatableStartingUp".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted((this.endStartupTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false));
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return null;
		}

		// Token: 0x06007A9E RID: 31390 RVA: 0x002BAAF8 File Offset: 0x002B8CF8
		public override void CompTick()
		{
			if (this.State == HibernatableStateDefOf.Starting && Find.TickManager.TicksGame > this.endStartupTick)
			{
				this.State = HibernatableStateDefOf.Running;
				this.endStartupTick = 0;
				string str;
				if (this.parent.Map.Parent.GetComponent<EscapeShipComp>() != null)
				{
					str = "LetterHibernateComplete".Translate();
				}
				else
				{
					str = "LetterHibernateCompleteStandalone".Translate();
				}
				Find.LetterStack.ReceiveLetter("LetterLabelHibernateComplete".Translate(), str, LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.parent), null, null, null, null, 0, true);
			}
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.sustainerActive.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
				return;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06007A9F RID: 31391 RVA: 0x002BAC1F File Offset: 0x002B8E1F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}

		// Token: 0x04004D6F RID: 19823
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x04004D70 RID: 19824
		private int endStartupTick;

		// Token: 0x04004D71 RID: 19825
		private Sustainer sustainer;
	}
}

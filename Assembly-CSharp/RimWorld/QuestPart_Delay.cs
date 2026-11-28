using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001047 RID: 4167
	public class QuestPart_Delay : QuestPartActivable
	{
		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x060068F2 RID: 26866 RVA: 0x002513EC File Offset: 0x0024F5EC
		public int TicksLeft
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return 0;
				}
				return this.enableTick + this.delayTicks - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x060068F3 RID: 26867 RVA: 0x00251411 File Offset: 0x0024F611
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.quest.Historical)
				{
					return null;
				}
				return this.expiryInfoPart.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x060068F4 RID: 26868 RVA: 0x00251447 File Offset: 0x0024F647
		public override string ExpiryInfoPartTip
		{
			get
			{
				return this.expiryInfoPartTip.Formatted(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.enableTick + this.delayTicks), QuestUtility.GetLocForDates()));
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x060068F5 RID: 26869 RVA: 0x0025147C File Offset: 0x0024F67C
		public override string AlertLabel
		{
			get
			{
				string text = this.alertLabel;
				TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(this.TicksLeft.ToStringTicksToPeriodVerbose(true, true))) : null;
				if (taggedString == null)
				{
					return null;
				}
				return taggedString.GetValueOrDefault();
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x060068F6 RID: 26870 RVA: 0x002514D4 File Offset: 0x0024F6D4
		public override string AlertExplanation
		{
			get
			{
				string text = this.alertExplanation;
				TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(this.TicksLeft.ToStringTicksToPeriodVerbose(true, true))) : null;
				if (taggedString == null)
				{
					return null;
				}
				return taggedString.GetValueOrDefault();
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x060068F7 RID: 26871 RVA: 0x0025152A File Offset: 0x0024F72A
		public override AlertReport AlertReport
		{
			get
			{
				if (this.alertCulprits.Count <= 0)
				{
					return AlertReport.Inactive;
				}
				return AlertReport.CulpritsAre(this.alertCulprits);
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x060068F8 RID: 26872 RVA: 0x0025154B File Offset: 0x0024F74B
		public override bool AlertCritical
		{
			get
			{
				return this.TicksLeft < this.ticksLeftAlertCritical;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x060068F9 RID: 26873 RVA: 0x0025155B File Offset: 0x0024F75B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Delay.<get_QuestLookTargets>d__26 <get_QuestLookTargets>d__ = new QuestPart_Delay.<get_QuestLookTargets>d__26(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x060068FA RID: 26874 RVA: 0x0025156B File Offset: 0x0024F76B
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.waitUntilPlayerHasHomeMap && Find.AnyPlayerHomeMap == null)
			{
				return;
			}
			if (Find.TickManager.TicksGame >= this.enableTick + this.delayTicks)
			{
				this.DelayFinished();
			}
		}

		// Token: 0x060068FB RID: 26875 RVA: 0x002515A2 File Offset: 0x0024F7A2
		protected virtual void DelayFinished()
		{
			base.Complete();
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x002515AC File Offset: 0x0024F7AC
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.inspectStringTargets != null && this.inspectStringTargets.Contains(target))
			{
				return this.inspectString.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
			}
			return null;
		}

		// Token: 0x060068FD RID: 26877 RVA: 0x002515FC File Offset: 0x0024F7FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPart, "expiryInfoPart", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartTip, "expiryInfoPartTip", null, false);
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Collections.Look<ISelectable>(ref this.inspectStringTargets, "inspectStringTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isBad, "isBad", false, false);
			Scribe_Values.Look<string>(ref this.alertLabel, "alertLabel", null, false);
			Scribe_Values.Look<string>(ref this.alertExplanation, "alertExplanation", null, false);
			Scribe_Values.Look<int>(ref this.ticksLeftAlertCritical, "ticksLeftAlertCritical", 0, false);
			Scribe_Values.Look<bool>(ref this.waitUntilPlayerHasHomeMap, "waitUntilPlayerHasHomeMap", false, false);
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.alertCulprits, "alertCulprits", LookMode.GlobalTargetInfo, Array.Empty<object>());
			if (this.alertCulprits == null)
			{
				this.alertCulprits = new List<GlobalTargetInfo>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.alertCulprits.RemoveAll((GlobalTargetInfo x) => !x.IsValid);
			}
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x00251724 File Offset: 0x0024F924
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true, null))
			{
				this.DelayFinished();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x060068FF RID: 26879 RVA: 0x00251791 File Offset: 0x0024F991
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x06006900 RID: 26880 RVA: 0x002517AE File Offset: 0x0024F9AE
		public void DebugForceEnd()
		{
			this.DelayFinished();
		}

		// Token: 0x040042D1 RID: 17105
		public int delayTicks;

		// Token: 0x040042D2 RID: 17106
		public string expiryInfoPart;

		// Token: 0x040042D3 RID: 17107
		public string expiryInfoPartTip;

		// Token: 0x040042D4 RID: 17108
		public string inspectString;

		// Token: 0x040042D5 RID: 17109
		public List<ISelectable> inspectStringTargets;

		// Token: 0x040042D6 RID: 17110
		public bool isBad;

		// Token: 0x040042D7 RID: 17111
		public bool waitUntilPlayerHasHomeMap;

		// Token: 0x040042D8 RID: 17112
		public string alertLabel;

		// Token: 0x040042D9 RID: 17113
		public string alertExplanation;

		// Token: 0x040042DA RID: 17114
		public List<GlobalTargetInfo> alertCulprits = new List<GlobalTargetInfo>();

		// Token: 0x040042DB RID: 17115
		public int ticksLeftAlertCritical;
	}
}

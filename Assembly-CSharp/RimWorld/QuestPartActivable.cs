using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110B RID: 4363
	public abstract class QuestPartActivable : QuestPart
	{
		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06006CED RID: 27885 RVA: 0x00262726 File Offset: 0x00260926
		public QuestPartState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06006CEE RID: 27886 RVA: 0x0026272E File Offset: 0x0026092E
		public int EnableTick
		{
			get
			{
				if (this.State != QuestPartState.Enabled)
				{
					return -1;
				}
				return this.enableTick;
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06006CEF RID: 27887 RVA: 0x00262744 File Offset: 0x00260944
		public string OutSignalEnabled
		{
			get
			{
				return string.Concat(new string[]
				{
					"Quest",
					this.quest.id.ToString(),
					".Part",
					base.Index.ToString(),
					".Enabled"
				});
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06006CF0 RID: 27888 RVA: 0x00262798 File Offset: 0x00260998
		public string OutSignalCompleted
		{
			get
			{
				return string.Concat(new string[]
				{
					"Quest",
					this.quest.id.ToString(),
					".Part",
					base.Index.ToString(),
					".Completed"
				});
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06006CF1 RID: 27889 RVA: 0x002627EC File Offset: 0x002609EC
		public virtual string ExpiryInfoPart { get; }

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06006CF2 RID: 27890 RVA: 0x002627F4 File Offset: 0x002609F4
		public virtual string ExpiryInfoPartTip { get; }

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06006CF3 RID: 27891 RVA: 0x002627FC File Offset: 0x002609FC
		public virtual AlertReport AlertReport
		{
			get
			{
				return AlertReport.Inactive;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06006CF4 RID: 27892 RVA: 0x00262803 File Offset: 0x00260A03
		public virtual string AlertLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06006CF5 RID: 27893 RVA: 0x00262806 File Offset: 0x00260A06
		public virtual string AlertExplanation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06006CF6 RID: 27894 RVA: 0x00262809 File Offset: 0x00260A09
		public virtual bool AlertCritical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06006CF7 RID: 27895 RVA: 0x0026280C File Offset: 0x00260A0C
		public virtual GlobalTargetInfo? AlertCulpritTarget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06006CF8 RID: 27896 RVA: 0x00262822 File Offset: 0x00260A22
		public bool AlertDirty
		{
			get
			{
				return this.cachedAlert != null && ((this.AlertCritical && !(this.cachedAlert is Alert_CustomCritical)) || (!this.AlertCritical && !(this.cachedAlert is Alert_Custom)));
			}
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06006CF9 RID: 27897 RVA: 0x00262860 File Offset: 0x00260A60
		public Alert CachedAlert
		{
			get
			{
				AlertReport alertReport = this.AlertReport;
				if (this.cachedAlert == null)
				{
					if (!alertReport.active)
					{
						return null;
					}
					if (this.AlertCritical)
					{
						this.cachedAlert = new Alert_CustomCritical();
					}
					else
					{
						this.cachedAlert = new Alert_Custom();
					}
				}
				Alert_Custom alert_Custom = this.cachedAlert as Alert_Custom;
				if (alert_Custom != null)
				{
					if (alertReport.active)
					{
						alert_Custom.label = this.AlertLabel;
						alert_Custom.explanation = this.AlertExplanation;
						alertReport.culpritTarget = this.AlertCulpritTarget;
					}
					alert_Custom.report = alertReport;
				}
				Alert_CustomCritical alert_CustomCritical = this.cachedAlert as Alert_CustomCritical;
				if (alert_CustomCritical != null)
				{
					if (alertReport.active)
					{
						alert_CustomCritical.label = this.AlertLabel;
						alert_CustomCritical.explanation = this.AlertExplanation;
					}
					alert_CustomCritical.report = alertReport;
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x06006CFA RID: 27898 RVA: 0x00262925 File Offset: 0x00260B25
		public virtual void QuestPartTick()
		{
		}

		// Token: 0x06006CFB RID: 27899 RVA: 0x00262927 File Offset: 0x00260B27
		public virtual string ExtraInspectString(ISelectable target)
		{
			return null;
		}

		// Token: 0x06006CFC RID: 27900 RVA: 0x0026292A File Offset: 0x00260B2A
		public virtual IEnumerable<Gizmo> ExtraGizmos(ISelectable target)
		{
			return null;
		}

		// Token: 0x06006CFD RID: 27901 RVA: 0x0026292D File Offset: 0x00260B2D
		public void ClearCachedAlert()
		{
			this.cachedAlert = null;
		}

		// Token: 0x06006CFE RID: 27902 RVA: 0x00262938 File Offset: 0x00260B38
		protected virtual void Enable(SignalArgs receivedArgs)
		{
			if (this.state == QuestPartState.Enabled)
			{
				Log.Error("Tried to enable QuestPart while already enabled. part=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.state = QuestPartState.Enabled;
			this.enableTick = Find.TickManager.TicksGame;
			Find.SignalManager.SendSignal(new Signal(this.OutSignalEnabled, false));
		}

		// Token: 0x06006CFF RID: 27903 RVA: 0x00262998 File Offset: 0x00260B98
		protected void Complete()
		{
			this.Complete(default(SignalArgs));
		}

		// Token: 0x06006D00 RID: 27904 RVA: 0x002629B4 File Offset: 0x00260BB4
		protected void Complete(NamedArgument signalArg1)
		{
			this.Complete(new SignalArgs(signalArg1));
		}

		// Token: 0x06006D01 RID: 27905 RVA: 0x002629C2 File Offset: 0x00260BC2
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2));
		}

		// Token: 0x06006D02 RID: 27906 RVA: 0x002629D1 File Offset: 0x00260BD1
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3));
		}

		// Token: 0x06006D03 RID: 27907 RVA: 0x002629E1 File Offset: 0x00260BE1
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3, NamedArgument signalArg4)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3, signalArg4));
		}

		// Token: 0x06006D04 RID: 27908 RVA: 0x002629F3 File Offset: 0x00260BF3
		protected void Complete(params NamedArgument[] signalArgs)
		{
			this.Complete(new SignalArgs(signalArgs));
		}

		// Token: 0x06006D05 RID: 27909 RVA: 0x00262A04 File Offset: 0x00260C04
		protected virtual void Complete(SignalArgs signalArgs)
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to end QuestPart but its state is not Active. part=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.state = QuestPartState.Disabled;
			if (this.outcomeCompletedSignalArg != QuestEndOutcome.Unknown)
			{
				signalArgs.Add(this.outcomeCompletedSignalArg.Named("OUTCOME"));
			}
			Find.SignalManager.SendSignal(new Signal(this.OutSignalCompleted, signalArgs, false));
			if (!this.outSignalsCompleted.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.outSignalsCompleted.Count; i++)
				{
					if (!this.outSignalsCompleted[i].NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalsCompleted[i], signalArgs, false));
					}
				}
			}
		}

		// Token: 0x06006D06 RID: 27910 RVA: 0x00262ACC File Offset: 0x00260CCC
		protected virtual void Disable()
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to disable QuestPart but its state is not enabled. part=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.state = QuestPartState.Disabled;
		}

		// Token: 0x06006D07 RID: 27911 RVA: 0x00262AFC File Offset: 0x00260CFC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable && (this.state == QuestPartState.NeverEnabled || (this.state == QuestPartState.Disabled && this.reactivatable)))
			{
				this.Enable(signal.args);
				return;
			}
			if (this.state == QuestPartState.Enabled)
			{
				if (signal.tag == this.inSignalDisable)
				{
					this.Disable();
					return;
				}
				this.ProcessQuestSignal(signal);
			}
		}

		// Token: 0x06006D08 RID: 27912 RVA: 0x00262B6C File Offset: 0x00260D6C
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		// Token: 0x06006D09 RID: 27913 RVA: 0x00262B6E File Offset: 0x00260D6E
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignalEnable = this.quest.InitiateSignal;
		}

		// Token: 0x06006D0A RID: 27914 RVA: 0x00262B87 File Offset: 0x00260D87
		public void DebugForceComplete()
		{
			this.Complete();
		}

		// Token: 0x06006D0B RID: 27915 RVA: 0x00262B90 File Offset: 0x00260D90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalEnable, "inSignalEnable", null, false);
			Scribe_Values.Look<string>(ref this.inSignalDisable, "inSignalDisable", null, false);
			Scribe_Values.Look<bool>(ref this.reactivatable, "reactivatable", false, false);
			if (Scribe.mode != LoadSaveMode.Saving || !this.outSignalsCompleted.NullOrEmpty<string>())
			{
				Scribe_Collections.Look<string>(ref this.outSignalsCompleted, "outSignalsCompleted", LookMode.Value, Array.Empty<object>());
			}
			Scribe_Values.Look<QuestEndOutcome>(ref this.outcomeCompletedSignalArg, "outcomeCompletedSignalArg", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<QuestPartState>(ref this.state, "state", QuestPartState.NeverEnabled, false);
			Scribe_Values.Look<int>(ref this.enableTick, "enableTick", -1, false);
		}

		// Token: 0x0400454D RID: 17741
		public string inSignalEnable;

		// Token: 0x0400454E RID: 17742
		public string inSignalDisable;

		// Token: 0x0400454F RID: 17743
		public bool reactivatable;

		// Token: 0x04004550 RID: 17744
		public List<string> outSignalsCompleted = new List<string>();

		// Token: 0x04004551 RID: 17745
		public QuestEndOutcome outcomeCompletedSignalArg;

		// Token: 0x04004552 RID: 17746
		private QuestPartState state;

		// Token: 0x04004553 RID: 17747
		protected int enableTick = -1;

		// Token: 0x04004554 RID: 17748
		private Alert cachedAlert;
	}
}

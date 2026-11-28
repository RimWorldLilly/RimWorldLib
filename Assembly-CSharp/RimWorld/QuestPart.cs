using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001109 RID: 4361
	public abstract class QuestPart : IExposable, ILoadReferenceable
	{
		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06006CCE RID: 27854 RVA: 0x002625B2 File Offset: 0x002607B2
		public virtual string DescriptionPart { get; }

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06006CCF RID: 27855 RVA: 0x002625BA File Offset: 0x002607BA
		public int Index
		{
			get
			{
				return this.quest.PartsListForReading.IndexOf(this);
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06006CD0 RID: 27856 RVA: 0x002625CD File Offset: 0x002607CD
		public virtual IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return Enumerable.Empty<GlobalTargetInfo>();
			}
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06006CD1 RID: 27857 RVA: 0x002625D4 File Offset: 0x002607D4
		public virtual string QuestSelectTargetsLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x002625D7 File Offset: 0x002607D7
		public virtual IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				return Enumerable.Empty<GlobalTargetInfo>();
			}
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x002625DE File Offset: 0x002607DE
		public virtual IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				return Enumerable.Empty<Faction>();
			}
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x002625E5 File Offset: 0x002607E5
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return Enumerable.Empty<Dialog_InfoCard.Hyperlink>();
			}
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06006CD5 RID: 27861 RVA: 0x002625EC File Offset: 0x002607EC
		public virtual bool IncreasesPopulation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06006CD6 RID: 27862 RVA: 0x002625EF File Offset: 0x002607EF
		public virtual bool RequiresAccepter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06006CD7 RID: 27863 RVA: 0x002625F2 File Offset: 0x002607F2
		public virtual bool PreventsAutoAccept
		{
			get
			{
				return this.RequiresAccepter;
			}
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x002625FA File Offset: 0x002607FA
		public virtual bool QuestPartReserves(Pawn p)
		{
			return false;
		}

		// Token: 0x06006CD9 RID: 27865 RVA: 0x002625FD File Offset: 0x002607FD
		public virtual bool QuestPartReserves(Faction f)
		{
			return false;
		}

		// Token: 0x06006CDA RID: 27866 RVA: 0x00262600 File Offset: 0x00260800
		public virtual bool QuestPartReserves(TransportShip ship)
		{
			return false;
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x00262603 File Offset: 0x00260803
		public virtual void Cleanup()
		{
		}

		// Token: 0x06006CDC RID: 27868 RVA: 0x00262605 File Offset: 0x00260805
		public virtual void AssignDebugData()
		{
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x00262607 File Offset: 0x00260807
		public virtual void PreQuestAccept()
		{
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x00262609 File Offset: 0x00260809
		public virtual void ExposeData()
		{
			Scribe_Values.Look<QuestPart.SignalListenMode>(ref this.signalListenMode, "signalListenMode", QuestPart.SignalListenMode.OngoingOnly, false);
			Scribe_Values.Look<string>(ref this.debugLabel, "debugLabel", null, false);
		}

		// Token: 0x06006CDF RID: 27871 RVA: 0x0026262F File Offset: 0x0026082F
		public virtual void Notify_QuestSignalReceived(Signal signal)
		{
		}

		// Token: 0x06006CE0 RID: 27872 RVA: 0x00262631 File Offset: 0x00260831
		public virtual void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
		}

		// Token: 0x06006CE1 RID: 27873 RVA: 0x00262633 File Offset: 0x00260833
		public virtual void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x00262635 File Offset: 0x00260835
		public virtual void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
		}

		// Token: 0x06006CE3 RID: 27875 RVA: 0x00262637 File Offset: 0x00260837
		public virtual void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
		}

		// Token: 0x06006CE4 RID: 27876 RVA: 0x00262639 File Offset: 0x00260839
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x06006CE5 RID: 27877 RVA: 0x0026263B File Offset: 0x0026083B
		public virtual void Notify_PawnDiscarded(Pawn pawn)
		{
		}

		// Token: 0x06006CE6 RID: 27878 RVA: 0x0026263D File Offset: 0x0026083D
		public virtual void Notify_PreCleanup()
		{
		}

		// Token: 0x06006CE7 RID: 27879 RVA: 0x0026263F File Offset: 0x0026083F
		public virtual void PostQuestAdded()
		{
		}

		// Token: 0x06006CE8 RID: 27880 RVA: 0x00262641 File Offset: 0x00260841
		public virtual void ReplacePawnReferences(Pawn replace, Pawn with)
		{
		}

		// Token: 0x06006CE9 RID: 27881 RVA: 0x00262643 File Offset: 0x00260843
		public virtual void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
		}

		// Token: 0x06006CEA RID: 27882 RVA: 0x00262648 File Offset: 0x00260848
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 3);
			defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral(" (quest.id=");
			Quest quest = this.quest;
			defaultInterpolatedStringHandler.AppendFormatted<int>((quest != null) ? quest.id : -1);
			defaultInterpolatedStringHandler.AppendLiteral(", index=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Index);
			string str = defaultInterpolatedStringHandler.ToStringAndClear();
			if (!this.debugLabel.NullOrEmpty())
			{
				str = str + ", debugLabel=" + this.debugLabel;
			}
			return str + ")";
		}

		// Token: 0x06006CEB RID: 27883 RVA: 0x002626E4 File Offset: 0x002608E4
		public string GetUniqueLoadID()
		{
			return "QuestPart_" + this.quest.id.ToString() + "_" + this.Index.ToString();
		}

		// Token: 0x04004545 RID: 17733
		public Quest quest;

		// Token: 0x04004546 RID: 17734
		public QuestPart.SignalListenMode signalListenMode;

		// Token: 0x04004547 RID: 17735
		public string debugLabel;

		// Token: 0x0200304A RID: 12362
		public enum SignalListenMode
		{
			// Token: 0x0400C414 RID: 50196
			OngoingOnly,
			// Token: 0x0400C415 RID: 50197
			NotYetAcceptedOnly,
			// Token: 0x0400C416 RID: 50198
			OngoingOrNotYetAccepted,
			// Token: 0x0400C417 RID: 50199
			HistoricalOnly,
			// Token: 0x0400C418 RID: 50200
			Always
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B2 RID: 4274
	public class QuestPart_DamageUntilDowned : QuestPart
	{
		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06006AEF RID: 27375 RVA: 0x00258D70 File Offset: 0x00256F70
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_DamageUntilDowned.<get_QuestLookTargets>d__4 <get_QuestLookTargets>d__ = new QuestPart_DamageUntilDowned.<get_QuestLookTargets>d__4(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006AF0 RID: 27376 RVA: 0x00258D80 File Offset: 0x00256F80
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						HealthUtility.DamageUntilDowned(this.pawns[i], this.allowBleedingWounds, null, null, null);
					}
				}
			}
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x00258DEC File Offset: 0x00256FEC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.allowBleedingWounds, "allowBleedingWounds", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x00258E6C File Offset: 0x0025706C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x00258EB1 File Offset: 0x002570B1
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040043F8 RID: 17400
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040043F9 RID: 17401
		public string inSignal;

		// Token: 0x040043FA RID: 17402
		public bool allowBleedingWounds = true;
	}
}

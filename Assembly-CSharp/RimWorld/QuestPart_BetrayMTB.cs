using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001090 RID: 4240
	public class QuestPart_BetrayMTB : QuestPart_MTB
	{
		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06006A3F RID: 27199 RVA: 0x002560C8 File Offset: 0x002542C8
		protected override float MTBDays
		{
			get
			{
				Map map = null;
				bool flag = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanBetray(this.pawns[i]))
					{
						flag = true;
						map = this.pawns[i].MapHeld;
						break;
					}
				}
				if (!flag)
				{
					return -1f;
				}
				int num = 0;
				int num2 = 0;
				List<Pawn> list = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].IsColonist)
					{
						num++;
						if (list[j].Downed)
						{
							num2++;
						}
					}
				}
				if (num <= 1)
				{
					return QuestPart_BetrayMTB.DownedColonistsExcept1PctToMTBDaysCurve.Evaluate(1f);
				}
				return QuestPart_BetrayMTB.DownedColonistsExcept1PctToMTBDaysCurve.Evaluate((float)num2 / (float)(num - 1));
			}
		}

		// Token: 0x06006A40 RID: 27200 RVA: 0x002561A1 File Offset: 0x002543A1
		private bool CanBetray(Pawn p)
		{
			return !p.DestroyedOrNull() && !p.Downed && p.IsFreeColonist && !p.InMentalState && !p.IsBurning() && !p.Suspended && p.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x002561DC File Offset: 0x002543DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400437C RID: 17276
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400437D RID: 17277
		private static readonly SimpleCurve DownedColonistsExcept1PctToMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 120f),
				true
			},
			{
				new CurvePoint(1f, 15f),
				true
			}
		};
	}
}

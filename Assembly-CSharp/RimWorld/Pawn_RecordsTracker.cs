using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A3 RID: 5539
	public class Pawn_RecordsTracker : IExposable
	{
		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x060088BA RID: 35002 RVA: 0x00317B5C File Offset: 0x00315D5C
		public Battle BattleActive
		{
			get
			{
				if (this.battleExitTick < Find.TickManager.TicksGame)
				{
					return null;
				}
				if (this.battleActive == null)
				{
					return null;
				}
				while (this.battleActive.AbsorbedBy != null)
				{
					this.battleActive = this.battleActive.AbsorbedBy;
				}
				return this.battleActive;
			}
		}

		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x060088BB RID: 35003 RVA: 0x00317BAB File Offset: 0x00315DAB
		public int LastBattleTick
		{
			get
			{
				return this.battleExitTick;
			}
		}

		// Token: 0x060088BC RID: 35004 RVA: 0x00317BB3 File Offset: 0x00315DB3
		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060088BD RID: 35005 RVA: 0x00317BCD File Offset: 0x00315DCD
		public void RecordsTickInterval(int delta)
		{
			if (this.pawn.Dead)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(80, delta))
			{
				this.RecordsTickUpdate(80);
				this.battleActive = this.BattleActive;
			}
		}

		// Token: 0x060088BE RID: 35006 RVA: 0x00317C01 File Offset: 0x00315E01
		public void RecordsTickMothballed(int interval)
		{
			this.RecordsTickUpdate(interval);
		}

		// Token: 0x060088BF RID: 35007 RVA: 0x00317C0C File Offset: 0x00315E0C
		private void RecordsTickUpdate(int interval)
		{
			List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].type == RecordType.Time && allDefsListForReading[i].Worker.ShouldMeasureTimeNow(this.pawn))
				{
					DefMap<RecordDef, float> defMap = this.records;
					RecordDef def = allDefsListForReading[i];
					defMap[def] += (float)interval;
				}
			}
		}

		// Token: 0x060088C0 RID: 35008 RVA: 0x00317C78 File Offset: 0x00315E78
		public void Increment(RecordDef def)
		{
			if (def.type != RecordType.Int)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to increment record \"",
					def.defName,
					"\" whose record type is \"",
					def.type.ToString(),
					"\"."
				}));
				return;
			}
			this.records[def] = Mathf.Round(this.records[def] + 1f);
		}

		// Token: 0x060088C1 RID: 35009 RVA: 0x00317CF8 File Offset: 0x00315EF8
		public void AddTo(RecordDef def, float value)
		{
			if (def.type == RecordType.Int)
			{
				this.records[def] = Mathf.Round(this.records[def] + Mathf.Round(value));
				return;
			}
			if (def.type == RecordType.Float)
			{
				DefMap<RecordDef, float> defMap = this.records;
				defMap[def] += value;
				return;
			}
			Log.Error(string.Concat(new string[]
			{
				"Tried to add value to record \"",
				def.defName,
				"\" whose record type is \"",
				def.type.ToString(),
				"\"."
			}));
		}

		// Token: 0x060088C2 RID: 35010 RVA: 0x00317D9C File Offset: 0x00315F9C
		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			if (def.type == RecordType.Int || def.type == RecordType.Time)
			{
				return Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x060088C3 RID: 35011 RVA: 0x00317DCF File Offset: 0x00315FCF
		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		// Token: 0x060088C4 RID: 35012 RVA: 0x00317DE2 File Offset: 0x00315FE2
		public void EnterBattle(Battle battle)
		{
			this.battleActive = battle;
			this.battleExitTick = Find.TickManager.TicksGame + 5000;
		}

		// Token: 0x060088C5 RID: 35013 RVA: 0x00317E04 File Offset: 0x00316004
		public void ExposeData()
		{
			this.battleActive = this.BattleActive;
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.battleActive, "battleActive", false);
			Scribe_Values.Look<int>(ref this.battleExitTick, "battleExitTick", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04005456 RID: 21590
		public Pawn pawn;

		// Token: 0x04005457 RID: 21591
		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		// Token: 0x04005458 RID: 21592
		private Battle battleActive;

		// Token: 0x04005459 RID: 21593
		private int battleExitTick;

		// Token: 0x0400545A RID: 21594
		private const int UpdateTimeRecordsIntervalTicks = 80;
	}
}

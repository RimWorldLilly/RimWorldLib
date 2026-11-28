using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001123 RID: 4387
	public sealed class RelationshipRecords : IExposable
	{
		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x06006DE6 RID: 28134 RVA: 0x0026814F File Offset: 0x0026634F
		public IReadOnlyDictionary<int, RelationshipRecord> Records
		{
			get
			{
				return this.records;
			}
		}

		// Token: 0x06006DE7 RID: 28135 RVA: 0x00268158 File Offset: 0x00266358
		public RelationshipRecord GetOrCreateRecord(Pawn pawn)
		{
			RelationshipRecord result;
			if (this.records.TryGetValue(pawn.thingIDNumber, out result))
			{
				return result;
			}
			return this.CreateRecord(pawn);
		}

		// Token: 0x06006DE8 RID: 28136 RVA: 0x00268184 File Offset: 0x00266384
		public RelationshipRecord CreateRecord(Pawn pawn)
		{
			RelationshipRecord relationshipRecord = new RelationshipRecord(pawn.thingIDNumber, pawn.gender, pawn.Name.ToStringShort);
			this.records.Add(relationshipRecord.ID, relationshipRecord);
			return relationshipRecord;
		}

		// Token: 0x06006DE9 RID: 28137 RVA: 0x002681C1 File Offset: 0x002663C1
		public RelationshipRecord GetRecord(int id)
		{
			return this.records[id];
		}

		// Token: 0x06006DEA RID: 28138 RVA: 0x002681D0 File Offset: 0x002663D0
		public int CleanupUnusedRecords()
		{
			RelationshipRecords.toRemove.Clear();
			foreach (KeyValuePair<int, RelationshipRecord> keyValuePair in this.records)
			{
				int num;
				RelationshipRecord relationshipRecord;
				keyValuePair.Deconstruct(out num, out relationshipRecord);
				int item = num;
				if (relationshipRecord.References.Count <= 1)
				{
					RelationshipRecords.toRemove.Add(item);
				}
			}
			foreach (int key in RelationshipRecords.toRemove)
			{
				foreach (Pawn pawn in this.records[key].References)
				{
					pawn.relations.RemoveRelation(this.records[key]);
				}
				this.records.Remove(key);
			}
			int count = RelationshipRecords.toRemove.Count;
			RelationshipRecords.toRemove.Clear();
			return count;
		}

		// Token: 0x06006DEB RID: 28139 RVA: 0x00268308 File Offset: 0x00266508
		public void ExposeData()
		{
			Scribe_Collections.Look<int, RelationshipRecord>(ref this.records, "records", LookMode.Value, LookMode.Deep, ref this.tmpKeys, ref this.tmpValues, true, false, false);
		}

		// Token: 0x04004645 RID: 17989
		private Dictionary<int, RelationshipRecord> records = new Dictionary<int, RelationshipRecord>();

		// Token: 0x04004646 RID: 17990
		private static readonly HashSet<int> toRemove = new HashSet<int>();

		// Token: 0x04004647 RID: 17991
		private List<int> tmpKeys;

		// Token: 0x04004648 RID: 17992
		private List<RelationshipRecord> tmpValues;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002028 RID: 8232
	public class DuplicateSet : IExposable
	{
		// Token: 0x1700257C RID: 9596
		// (get) Token: 0x0600D071 RID: 53361 RVA: 0x004CA585 File Offset: 0x004C8785
		public int Count
		{
			get
			{
				return this.pawns.Count;
			}
		}

		// Token: 0x0600D072 RID: 53362 RVA: 0x004CA592 File Offset: 0x004C8792
		public void Add(Pawn pawn)
		{
			this.pawns.Add(pawn);
		}

		// Token: 0x0600D073 RID: 53363 RVA: 0x004CA5A1 File Offset: 0x004C87A1
		public void Remove(Pawn pawn)
		{
			this.pawns.Remove(pawn);
		}

		// Token: 0x0600D074 RID: 53364 RVA: 0x004CA5B0 File Offset: 0x004C87B0
		public bool Contains(Pawn pawn)
		{
			return this.pawns.Contains(pawn);
		}

		// Token: 0x0600D075 RID: 53365 RVA: 0x004CA5BE File Offset: 0x004C87BE
		public void Clear()
		{
			this.pawns.Clear();
		}

		// Token: 0x0600D076 RID: 53366 RVA: 0x004CA5CB File Offset: 0x004C87CB
		public Pawn First()
		{
			return this.pawns.First<Pawn>();
		}

		// Token: 0x0600D077 RID: 53367 RVA: 0x004CA5D8 File Offset: 0x004C87D8
		public HashSet<Pawn>.Enumerator GetEnumerator()
		{
			return this.pawns.GetEnumerator();
		}

		// Token: 0x0600D078 RID: 53368 RVA: 0x004CA5E5 File Offset: 0x004C87E5
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pawns.Contains(null))
			{
				this.pawns.Remove(null);
			}
		}

		// Token: 0x040084A5 RID: 33957
		private HashSet<Pawn> pawns = new HashSet<Pawn>();
	}
}

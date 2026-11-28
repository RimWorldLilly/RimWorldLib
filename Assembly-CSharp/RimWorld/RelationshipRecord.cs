using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001122 RID: 4386
	public class RelationshipRecord : IExposable, ILoadReferenceable
	{
		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06006DDC RID: 28124 RVA: 0x00267FF0 File Offset: 0x002661F0
		public int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06006DDD RID: 28125 RVA: 0x00267FF8 File Offset: 0x002661F8
		public Gender Gender
		{
			get
			{
				return this.gender;
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06006DDE RID: 28126 RVA: 0x00268000 File Offset: 0x00266200
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x06006DDF RID: 28127 RVA: 0x00268008 File Offset: 0x00266208
		public IReadOnlyCollection<Pawn> References
		{
			get
			{
				if (this.references.Contains(null))
				{
					this.references.Remove(null);
				}
				return this.references;
			}
		}

		// Token: 0x06006DE0 RID: 28128 RVA: 0x0026802B File Offset: 0x0026622B
		public RelationshipRecord()
		{
		}

		// Token: 0x06006DE1 RID: 28129 RVA: 0x0026803E File Offset: 0x0026623E
		public RelationshipRecord(int id, Gender gender, string name)
		{
			this.id = id;
			this.gender = gender;
			this.name = name;
		}

		// Token: 0x06006DE2 RID: 28130 RVA: 0x00268066 File Offset: 0x00266266
		public void AddReference(Pawn pawn)
		{
			if (pawn == null)
			{
				Log.Error("Attempted to create relationship record reference from a pawn which is null!");
				return;
			}
			this.references.Add(pawn);
		}

		// Token: 0x06006DE3 RID: 28131 RVA: 0x00268083 File Offset: 0x00266283
		public void RemoveReference(Pawn pawn)
		{
			if (this.references.Contains(pawn))
			{
				this.references.Remove(pawn);
			}
		}

		// Token: 0x06006DE4 RID: 28132 RVA: 0x002680A0 File Offset: 0x002662A0
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Collections.Look<Pawn>(ref this.references, true, "references", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.references.Contains(null))
			{
				this.references.Remove(null);
			}
		}

		// Token: 0x06006DE5 RID: 28133 RVA: 0x00268118 File Offset: 0x00266318
		public string GetUniqueLoadID()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("RelationshipRecord_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.ID);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04004641 RID: 17985
		private int id;

		// Token: 0x04004642 RID: 17986
		private Gender gender;

		// Token: 0x04004643 RID: 17987
		private string name;

		// Token: 0x04004644 RID: 17988
		private HashSet<Pawn> references = new HashSet<Pawn>();
	}
}

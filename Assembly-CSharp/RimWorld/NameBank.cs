using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CC RID: 5324
	public class NameBank
	{
		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x06008234 RID: 33332 RVA: 0x002E9BC5 File Offset: 0x002E7DC5
		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				NameBank.<get_AllNameLists>d__5 <get_AllNameLists>d__ = new NameBank.<get_AllNameLists>d__5(-2);
				<get_AllNameLists>d__.<>4__this = this;
				return <get_AllNameLists>d__;
			}
		}

		// Token: 0x06008235 RID: 33333 RVA: 0x002E9BD8 File Offset: 0x002E7DD8
		public NameBank(PawnNameCategory ID)
		{
			this.nameType = ID;
			this.names = new List<string>[NameBank.numGenders, NameBank.numSlots];
			for (int i = 0; i < NameBank.numGenders; i++)
			{
				for (int j = 0; j < NameBank.numSlots; j++)
				{
					this.names[i, j] = new List<string>();
				}
			}
		}

		// Token: 0x06008236 RID: 33334 RVA: 0x002E9C3C File Offset: 0x002E7E3C
		public void ErrorCheck()
		{
			foreach (List<string> list in this.AllNameLists)
			{
				foreach (string str in (from x in list
				group x by x into g
				where g.Count<string>() > 1
				select g.Key).ToList<string>())
				{
					Log.Error("Duplicated name: " + str);
				}
				foreach (string text in list)
				{
					if (text.Trim() != text)
					{
						Log.Error("Trimmable whitespace on name: [" + text + "]");
					}
				}
			}
		}

		// Token: 0x06008237 RID: 33335 RVA: 0x002E9DC0 File Offset: 0x002E7FC0
		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		// Token: 0x06008238 RID: 33336 RVA: 0x002E9DD0 File Offset: 0x002E7FD0
		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		// Token: 0x06008239 RID: 33337 RVA: 0x002E9E20 File Offset: 0x002E8020
		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		// Token: 0x0600823A RID: 33338 RVA: 0x002E9E3C File Offset: 0x002E803C
		public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			if (list.Count == 0)
			{
				Log.Error(string.Concat(new string[]
				{
					"Name list for gender=",
					gender.ToString(),
					" slot=",
					slot.ToString(),
					" is empty."
				}));
				return "Errorname";
			}
			string text;
			for (;;)
			{
				text = list.RandomElement<string>();
				if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
				{
					break;
				}
				num++;
				if (num > 50)
				{
					return text;
				}
			}
			return text;
		}

		// Token: 0x040050A1 RID: 20641
		public PawnNameCategory nameType;

		// Token: 0x040050A2 RID: 20642
		private List<string>[,] names;

		// Token: 0x040050A3 RID: 20643
		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		// Token: 0x040050A4 RID: 20644
		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;
	}
}

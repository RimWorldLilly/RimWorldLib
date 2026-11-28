using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020006A1 RID: 1697
	public class DefMap<D, V> : IExposable, IEnumerable<KeyValuePair<D, V>>, IEnumerable where D : Def, new() where V : new()
	{
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x0600358E RID: 13710 RVA: 0x0015B4F8 File Offset: 0x001596F8
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x17000A04 RID: 2564
		public V this[D def]
		{
			get
			{
				return this.values[(int)def.index];
			}
			set
			{
				this.values[(int)def.index] = value;
			}
		}

		// Token: 0x17000A05 RID: 2565
		public V this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x0015B554 File Offset: 0x00159754
		public DefMap()
		{
			int defCount = DefDatabase<D>.DefCount;
			if (defCount == 0)
			{
				string[] array = new string[5];
				array[0] = "Constructed DefMap<";
				int num = 1;
				Type typeFromHandle = typeof(D);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[2] = ", ";
				int num2 = 3;
				Type typeFromHandle2 = typeof(V);
				array[num2] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
				array[4] = "> without defs being initialized. Try constructing it in ResolveReferences instead of the constructor.";
				throw new Exception(string.Concat(array));
			}
			this.values = new List<V>(defCount);
			for (int i = 0; i < defCount; i++)
			{
				this.values.Add(Activator.CreateInstance<V>());
			}
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x0015B5F4 File Offset: 0x001597F4
		public void ExposeData()
		{
			Scribe_Collections.Look<V>(ref this.values, "vals", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int defCount = DefDatabase<D>.DefCount;
				for (int i = this.values.Count; i < defCount; i++)
				{
					this.values.Add(Activator.CreateInstance<V>());
				}
				while (this.values.Count > defCount)
				{
					this.values.RemoveLast<V>();
				}
			}
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x0015B668 File Offset: 0x00159868
		public void SetAll(V val)
		{
			for (int i = 0; i < this.values.Count; i++)
			{
				this.values[i] = val;
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x0015B698 File Offset: 0x00159898
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x0015B6A0 File Offset: 0x001598A0
		public IEnumerator<KeyValuePair<D, V>> GetEnumerator()
		{
			return (from d in DefDatabase<D>.AllDefsListForReading
			select new KeyValuePair<D, V>(d, this[d])).GetEnumerator();
		}

		// Token: 0x04002301 RID: 8961
		private List<V> values;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006B6 RID: 1718
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x0600369F RID: 13983 RVA: 0x0015EA24 File Offset: 0x0015CC24
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060036A0 RID: 13984 RVA: 0x0015EA2C File Offset: 0x0015CC2C
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x0015EA34 File Offset: 0x0015CC34
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x0015EA44 File Offset: 0x0015CC44
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			T1 t = this.First;
			array[num] = t.ToString();
			array[2] = ", ";
			int num2 = 3;
			T2 t2 = this.Second;
			array[num2] = t2.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0015EAA2 File Offset: 0x0015CCA2
		public override int GetHashCode()
		{
			return Gen.HashCombine<T2>(Gen.HashCombine<T1>(0, this.first), this.second);
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x0015EABB File Offset: 0x0015CCBB
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x0015EAD3 File Offset: 0x0015CCD3
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x0015EB05 File Offset: 0x0015CD05
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x0015EB0F File Offset: 0x0015CD0F
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04002345 RID: 9029
		private T1 first;

		// Token: 0x04002346 RID: 9030
		private T2 second;
	}
}

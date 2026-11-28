using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A2 RID: 1698
	internal class Deque<T>
	{
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003599 RID: 13721 RVA: 0x0015B6CC File Offset: 0x001598CC
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0015B6D7 File Offset: 0x001598D7
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x0015B6FC File Offset: 0x001598FC
		public void PushFront(T item)
		{
			this.PushPrep();
			this.first--;
			if (this.first < 0)
			{
				this.first += this.data.Length;
			}
			this.count++;
			this.data[this.first] = item;
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x0015B75C File Offset: 0x0015995C
		public void PushBack(T item)
		{
			this.PushPrep();
			T[] array = this.data;
			int num = this.first;
			int num2 = this.count;
			this.count = num2 + 1;
			array[(num + num2) % this.data.Length] = item;
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x0015B79C File Offset: 0x0015999C
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x0015B7F9 File Offset: 0x001599F9
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x0015B80C File Offset: 0x00159A0C
		private void PushPrep()
		{
			if (this.count < this.data.Length)
			{
				return;
			}
			T[] destinationArray = new T[this.data.Length * 2];
			Array.Copy(this.data, this.first, destinationArray, 0, Mathf.Min(this.count, this.data.Length - this.first));
			if (this.first + this.count > this.data.Length)
			{
				Array.Copy(this.data, 0, destinationArray, this.data.Length - this.first, this.count - this.data.Length + this.first);
			}
			this.data = destinationArray;
			this.first = 0;
		}

		// Token: 0x04002302 RID: 8962
		private T[] data;

		// Token: 0x04002303 RID: 8963
		private int first;

		// Token: 0x04002304 RID: 8964
		private int count;
	}
}

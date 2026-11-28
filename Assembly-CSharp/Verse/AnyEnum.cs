using System;

namespace Verse
{
	// Token: 0x0200008D RID: 141
	public struct AnyEnum
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x00025740 File Offset: 0x00023940
		public T? As<T>() where T : struct
		{
			if (typeof(T) == this.enumType)
			{
				return new T?(GenericConverter.Convert<int, T>(this.enumCode));
			}
			return null;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00025780 File Offset: 0x00023980
		public static AnyEnum None
		{
			get
			{
				return default(AnyEnum);
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00025798 File Offset: 0x00023998
		public static AnyEnum FromEnum<T>(T reasonCode) where T : struct, Enum
		{
			return new AnyEnum
			{
				enumType = typeof(T),
				enumCode = GenericConverter.Convert<T, int>(reasonCode)
			};
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000257CC File Offset: 0x000239CC
		public override string ToString()
		{
			return Enum.ToObject(this.enumType, this.enumCode).ToStringSafe<object>();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000257E4 File Offset: 0x000239E4
		public static bool operator ==(AnyEnum lhs, AnyEnum rhs)
		{
			return lhs.enumCode == rhs.enumCode && lhs.enumType == rhs.enumType;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00025807 File Offset: 0x00023A07
		public static bool operator !=(AnyEnum lhs, AnyEnum rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00025814 File Offset: 0x00023A14
		public override bool Equals(object obj)
		{
			if (obj is AnyEnum)
			{
				AnyEnum rhs = (AnyEnum)obj;
				return this == rhs;
			}
			return false;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002583E File Offset: 0x00023A3E
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombine<Type>(0, this.enumType), this.enumCode);
		}

		// Token: 0x04000219 RID: 537
		public Type enumType;

		// Token: 0x0400021A RID: 538
		public int enumCode;
	}
}

using System;

namespace Verse
{
	// Token: 0x02000293 RID: 659
	public struct ThingRequest
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00070A02 File Offset: 0x0006EC02
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00070A17 File Offset: 0x0006EC17
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00070A44 File Offset: 0x0006EC44
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00070A6C File Offset: 0x0006EC6C
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00070A94 File Offset: 0x0006EC94
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x00070ABA File Offset: 0x0006ECBA
		public bool Accepts(Thing t)
		{
			if (this.singleDef != null)
			{
				return t.def == this.singleDef;
			}
			return this.group == ThingRequestGroup.Everything || this.group.Includes(t.def);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00070AF0 File Offset: 0x0006ECF0
		public override string ToString()
		{
			string str;
			if (this.singleDef != null)
			{
				str = "singleDef " + this.singleDef.defName;
			}
			else
			{
				str = "group " + this.group.ToString();
			}
			return "ThingRequest(" + str + ")";
		}

		// Token: 0x04001124 RID: 4388
		public ThingDef singleDef;

		// Token: 0x04001125 RID: 4389
		public ThingRequestGroup group;
	}
}

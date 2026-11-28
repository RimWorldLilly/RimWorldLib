using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001487 RID: 5255
	public class CreepJoinerWorker_MetalhorrorDownside : BaseCreepJoinerWorker
	{
		// Token: 0x060080F4 RID: 33012 RVA: 0x002DFEF3 File Offset: 0x002DE0F3
		public override void OnCreated()
		{
			MetalhorrorUtility.Infect(base.Pawn, null, "InsectImplant", null);
		}

		// Token: 0x060080F5 RID: 33013 RVA: 0x002DFF07 File Offset: 0x002DE107
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
		}
	}
}

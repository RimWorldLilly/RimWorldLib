using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D48 RID: 3400
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		// Token: 0x06005F0D RID: 24333 RVA: 0x0022B8A5 File Offset: 0x00229AA5
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}

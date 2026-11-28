using System;

namespace RimWorld
{
	// Token: 0x0200120A RID: 4618
	public static class StoryDangerUtility
	{
		// Token: 0x060072B6 RID: 29366 RVA: 0x00281B64 File Offset: 0x0027FD64
		public static float Scale(this StoryDanger d)
		{
			switch (d)
			{
			case StoryDanger.None:
				return 0f;
			case StoryDanger.Low:
				return 1f;
			case StoryDanger.High:
				return 2f;
			default:
				return 0f;
			}
		}
	}
}

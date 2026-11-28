using System;

namespace Verse
{
	// Token: 0x02000715 RID: 1813
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x060039B3 RID: 14771 RVA: 0x0016F998 File Offset: 0x0016DB98
		public static string ToStringHuman(this TemperatureDisplayMode mode)
		{
			switch (mode)
			{
			case TemperatureDisplayMode.Celsius:
				return "Celsius".Translate();
			case TemperatureDisplayMode.Fahrenheit:
				return "Fahrenheit".Translate();
			case TemperatureDisplayMode.Kelvin:
				return "Kelvin".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}

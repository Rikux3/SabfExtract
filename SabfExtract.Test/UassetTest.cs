using System;
using System.IO;
using Xunit;

namespace SabfExtract.Test
{
    public class UassetTest
    {
        [Theory]
        [InlineData(@"F:\CUSA05787\tresgame\content\paks\_Ex\TresGame\Content\Sound\VOICE\Localization\en\Field\Rebellis\fd_dw_aqua_002.uasset")]
        public void ParseUasset(string filePath)
        {
            Uasset asset = new Uasset(File.OpenRead(filePath));
        }
    }
}

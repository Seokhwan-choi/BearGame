using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Bear
{
	static public class LCompressor
	{
		//===================================
		// Byte Compress
		//===================================
		static public byte[] Compress(byte[] bytes)
		{
			using (var dest = new MemoryStream())
			{
				using (var ds = new DeflateStream(dest, CompressionMode.Compress))
				{
					ds.Write(bytes, 0, bytes.Length);
				}

				return dest.ToArray();
			}
		}

		static public byte[] Decompress(byte[] bytes)
		{
			using (var dest = new MemoryStream())
			{
				using (var src = new MemoryStream(bytes))
				{
					using (var ds = new DeflateStream(src, CompressionMode.Decompress))
					{
						ds.CopyTo(dest);
					}
				}

				return dest.ToArray();
			}
		}
	}
}
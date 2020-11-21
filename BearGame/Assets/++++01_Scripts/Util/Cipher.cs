using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bear
{
	public static class LCipher
	{
		public static byte[] Encrypt(byte[] bytes, byte[] key)
		{
			int len = bytes.Length;
			int keyLen = key.Length;

			int i = 0;
			int m = 0;

			for (; i <= len - 3; i += 3)
			{
				byte xor = key[m++];
				if (m == key.Length)
					m = 0;

				byte t0 = bytes[i + 0];
				byte t1 = bytes[i + 1];
				byte t2 = bytes[i + 2];

				bytes[i + 0] = (byte)(t2 ^ xor);
				bytes[i + 1] = (byte)(t0 ^ xor);
				bytes[i + 2] = (byte)(t1 ^ xor);
			}

			for (; i < len; i++)
			{
				bytes[i] ^= key[m++];
				if (m == key.Length)
					m = 0;
			}

			return bytes;
		}

		public static byte[] Decrypt(byte[] bytes, byte[] key)
		{
			int len = bytes.Length;

			int i = 0;
			int m = 0;

			for (; i <= len - 3; i += 3)
			{
				byte xor = key[m++];
				if (m == key.Length)
					m = 0;

				byte t0 = bytes[i + 0];
				byte t1 = bytes[i + 1];
				byte t2 = bytes[i + 2];

				bytes[i + 0] = (byte)(t1 ^ xor);
				bytes[i + 1] = (byte)(t2 ^ xor);
				bytes[i + 2] = (byte)(t0 ^ xor);
			}

			for (; i < len; i++)
			{
				bytes[i] ^= key[m++];
				if (m == key.Length)
					m = 0;
			}

			return bytes;
		}

		public static byte[] Shuffle(byte[] bytes)
		{
			int len = bytes.Length;

			int head = 0;
			int tail = len - 1;

			while (head < tail)
			{
				byte t = bytes[head];
				bytes[head] = bytes[tail];
				bytes[tail] = t;

				head += 2;
				tail -= 2;
			}

			return bytes;
		}

		public static byte[] ShuffledEncrypt(byte[] bytes, byte[] key)
		{
			return Encrypt(Shuffle(bytes), key);
		}

		public static byte[] ShuffledDecrypt(byte[] bytes, byte[] key)
		{
			return Shuffle(Decrypt(bytes, key));
		}


		// Helper functions
		public static byte[] GenerateNewKey()
		{
			return Guid.NewGuid().ToByteArray();
		}

#if !NO_LJSON
		public static byte[] EncodeObject(object obj, byte[] key, bool compress)
		{
			string json = LJson.ToJson(obj);
			byte[] utf8 = Encoding.UTF8.GetBytes(json);
			byte[] compressed = compress ? LCompressor.Compress(utf8) : utf8;

			byte[] encrypted = ShuffledEncrypt(compressed, key);
			return encrypted;
		}

		public static object DecodeObject(byte[] crypted, Type type, byte[] key, bool compressed)
		{
			byte[] decrypted = ShuffledDecrypt(crypted, key);

			byte[] utf8 = compressed ? LCompressor.Decompress(decrypted) : decrypted;
			string json = Encoding.UTF8.GetString(utf8);

			return LJson.FromJson(json, type);
		}

		public static T DecodeObject<T>(byte[] crypted, byte[] key, bool compressed)
		{
			return (T)DecodeObject(crypted, typeof(T), key, compressed);
		}

		public static T SafeDecodeObject<T>(byte[] crypted, byte[] key, bool compressed)
		{
			try
			{
				return (T)DecodeObject(crypted, typeof(T), key, compressed);
			}
			catch { }

			return default(T);
		}
#endif

		public static string SimpleEncryptString(string s, string key)
		{
			byte[] keyBytes = Encoding.UTF8.GetBytes(key);
			byte[] plainBytes = Encoding.UTF8.GetBytes(s);
			byte[] cryptBytes = ShuffledEncrypt(plainBytes, keyBytes);

			return Convert.ToBase64String(cryptBytes);
		}

		public static string SimpleDecryptString(string s, string key)
		{
			byte[] keyBytes = Encoding.UTF8.GetBytes(key);
			byte[] cryptBytes = Convert.FromBase64String(s);
			byte[] plainBytes = ShuffledDecrypt(cryptBytes, keyBytes);

			return Encoding.UTF8.GetString(plainBytes);
		}
	}
}
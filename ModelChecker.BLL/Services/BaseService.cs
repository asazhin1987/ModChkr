using Bimacad.Sys;
using ModelChecker.DAL.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ModelChecker.BLL.Services
{
	public abstract class BaseService
	{
		protected readonly IUnitOfWork db;

		public BaseService(IUnitOfWork uw)
		{
			db = uw;
		}

		public bool CheckLicense()
		{
			var lic = db.License.Get(0);
			if (lic == null)
				return false;
			string val = GetDecryptVal(lic.Key);
			var date = GetLicenseDateByDecryptStr(val);
			var qnt = GetLicenseCountByDecryptStr(val);
			if (date == null || date.Value < DateTime.Now.Date || qnt == 0)
				return false;
			return true;
		}

		public string GetDelimiter()
		{
			var par = db.Params.Get(0);
			return par.Delimiter;
		}

		public async Task<string> GetDelimiterAsync()
		{
			var par = await db.Params.GetAsync(0);
			return par.Delimiter;
		}

		internal static string GetDecryptVal(string licvalue) =>
			Decrypt(licvalue, Resources.ApplicationRes.Key);

		internal static DateTime? GetLicenseDate(string licvalue) =>
			GetLicenseDateByDecryptStr(GetDecryptVal(licvalue));

		internal static int GetLicenseQnt(string licvalue) =>
			GetLicenseQnt(GetDecryptVal(licvalue));


		internal (DateTime? date, int qnt) GetLicenseParamsAsync(string key)
		{
			string sval = GetDecryptVal(key);
			return (GetLicenseDateByDecryptStr(sval), GetLicenseCountByDecryptStr(sval));
		}

		internal static DateTime? GetLicenseDateByDecryptStr(string licvalue)
		{
			try
			{
				if (DateTime.TryParse(licvalue.Substring(0, 10), out DateTime date))
					return date;
				return null;
			}
			catch
			{
				return null;
			}
			
		}

		internal static int GetLicenseCountByDecryptStr(string licvalue)
		{
			try
			{
				if (licvalue.Contains("<qnt>"))
				{
					if (int.TryParse(licvalue.Substring(licvalue.IndexOf("<qnt>") + 5), out int qnt))
						return qnt;
				}
				return 0;
			}
			catch
			{
				return 0;
			}
		}

		private static CryptoStream InternalDecrypt(byte[] key, string value)
		{
			SymmetricAlgorithm sa = Rijndael.Create();
			ICryptoTransform ct = sa.CreateDecryptor(new PasswordDeriveBytes(value, null).GetBytes(16), new byte[16]);

			MemoryStream ms = new MemoryStream(key);
			return new CryptoStream(ms, ct, CryptoStreamMode.Read);
		}

		private static string Decrypt(string str, string keyCrypt)
		{
			string Result;
			try
			{
				using (CryptoStream Cs = InternalDecrypt(Convert.FromBase64String(str), keyCrypt))
				{
					using (StreamReader Sr = new StreamReader(Cs))
					{
						Result = Sr.ReadToEnd();
						Sr.Close();
					}
					Cs.Close();
				}

			}
			catch (CryptographicException)
			{
				return null;
			}
			catch (Exception)
			{
				return null;
			}
			return Result;
		}
	}
}

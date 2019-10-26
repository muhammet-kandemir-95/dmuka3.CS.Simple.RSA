using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace dmuka3.CS.Simple.RSA
{
    /// <summary>
    /// This class provide you that gets a private and public key by rsa.
    /// Then, you can use them to hide your datas.
    /// Also, you can encrypt data by public key.
    /// </summary>
    public class RSAKey
    {
        #region Variables
        /// <summary>
        /// Public key as xml.
        /// </summary>
        public string PublicKey { get; private set; }

        /// <summary>
        /// Max data length for a encrypt.
        /// </summary>
        private int _dataMaxLength = 0;

        /// <summary>
        /// Encrypted a package size.
        /// </summary>
        private int _encryptedPackageSize = 0;

        int _dwKeySize = 0;
        /// <summary>
        /// Key size as bit.
        /// </summary>
        public int DwKeySize
        {
            get
            {
                return this._dwKeySize;
            }
            private set
            {
                this._dwKeySize = value;
                this._dataMaxLength = ((this._dwKeySize - 384) / 8) + 6;
                this._encryptedPackageSize = this.DwKeySize / 8;
            }
        }

        /// <summary>
        /// For rsa processes.
        /// </summary>
        private RSACryptoServiceProvider _provider = null;

        /// <summary>
        /// RSA private and public key.
        /// </summary>
        private RSAParameters
                        _privateKey,
                        _publicKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a rsa key with private and public key.
        /// </summary>
        /// <param name="dwKeySize">Key size as bit.</param>
        public RSAKey(int dwKeySize)
        {
            this.DwKeySize = dwKeySize;

            this._provider = new RSACryptoServiceProvider(this.DwKeySize);
            this._privateKey = this._provider.ExportParameters(true);
            this._publicKey = this._provider.ExportParameters(false);

            var sw = new System.IO.StringWriter();
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, this._publicKey);
            this.PublicKey = sw.ToString();
        }

        /// <summary>
        /// Read a rsa key with public key.
        /// </summary>
        /// <param name="publicKey">Public key of rsa.</param>
        public RSAKey(string publicKey)
        {
            this.PublicKey = publicKey;

            var sr = new System.IO.StringReader(this.PublicKey);
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            this._publicKey = (RSAParameters)xs.Deserialize(sr);

            this._provider = new RSACryptoServiceProvider();
            this._provider.ImportParameters(this._publicKey);

            this.DwKeySize = this._provider.KeySize;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Encrypt data by RSA.
        /// </summary>
        /// <param name="buffer">Original data.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] buffer)
        {
            if (buffer.Length <= this._dataMaxLength)
                return this._provider.Encrypt(buffer, true);

            List<byte> encryptedBuffer = new List<byte>();
            for (int i = 0; i < buffer.Length; i += this._dataMaxLength)
            {
                byte[] part = new byte[Math.Min(this._dataMaxLength, buffer.Length - i)];
                Array.Copy(buffer, i, part, 0, part.Length);
                encryptedBuffer.AddRange(this._provider.Encrypt(part, true));
            }

            var encryptedBufferAsArray = encryptedBuffer.ToArray();
            encryptedBuffer.Clear();
            return encryptedBufferAsArray;
        }

        /// <summary>
        /// Decrypt data by RSA.
        /// </summary>
        /// <param name="buffer">Original data.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] buffer)
        {
            List<byte> decryptedBuffer = new List<byte>();
            for (int i = 0; i < buffer.Length; i += this._encryptedPackageSize)
            {
                byte[] part = new byte[this._encryptedPackageSize];
                Array.Copy(buffer, i, part, 0, part.Length);
                decryptedBuffer.AddRange(this._provider.Decrypt(part, true));
            }

            var decryptedBufferAsArray = decryptedBuffer.ToArray();
            decryptedBuffer.Clear();

            return decryptedBufferAsArray;
        }
        #endregion
    }
}

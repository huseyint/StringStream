using System;
using System.IO;
using System.Text;

namespace StringStreamSample
{
	public class StringStream : Stream
	{
		private readonly string _string;

		private readonly Encoding _encoding;

		private readonly long _byteLength;

		private int _position;

		public StringStream(string str, Encoding encoding = null)
		{
			_string = str ?? string.Empty;
			_encoding = encoding ?? Encoding.UTF8;

			_byteLength = _encoding.GetByteCount(_string);
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override long Length
		{
			get { return _byteLength; }
		}

		public override long Position
		{
			get
			{
				return _position;
			}

			set
			{
				if (value < 0 || value > int.MaxValue)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				_position = (int)value;
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin:
					Position = offset;
					break;
				case SeekOrigin.End:
					Position = _byteLength + offset;
					break;
				case SeekOrigin.Current:
					Position += offset;
					break;
			}

			return Position;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_position < 0)
			{
				throw new InvalidOperationException();
			}

			var bytesRead = 0;
			var chars = new char[1];

			// Loop until the buffer is full or the string has no more chars
			while (bytesRead < count && _position < _string.Length)
			{
				// Get the current char to encode
				chars[0] = _string[_position];

				// Get the required byte count for current char
				var byteCount = _encoding.GetByteCount(chars);

				// If adding current char to buffer will exceed its length, do not add it
				if (bytesRead + byteCount > count)
				{
					return bytesRead;
				}

				// Add the bytes of current char to byte buffer at next index
				_encoding.GetBytes(chars, 0, 1, buffer, offset + bytesRead);

				// Increment the string position and total bytes read so far
				Position++;
				bytesRead += byteCount;
			}

			return bytesRead;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}
	}
}
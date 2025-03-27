# PrivMX Endpoint C#

This repository provides C# wrapper for the native C++ library used by PrivMX to handle
end-to-end (e2e) encryption. PrivMX is a privacy-focused platform designed to offer secure
collaboration solutions by integrating robust encryption across various data types and communication
methods. This project enables seamless integration of PrivMX’s encryption functionalities in
C# applications, preserving the security and performance of the original C++ library while making its
capabilities accessible on the .NET platform.

## About PrivMX

[PrivMX](https://privmx.dev) allows developers to build end-to-end encrypted apps used for
communication. The Platform works according to privacy-by-design mindset, so all of our solutions
are based on Zero-Knowledge architecture. This project extends PrivMX’s commitment to security by
making its encryption features accessible to developers using C#.

### Key Features

- End-to-End Encryption: Ensures that data is encrypted at the source and can only be decrypted by
  the intended recipient.
- Native C++ Library Integration: Leverages the performance and security of C++ while making it
  accessible in C# applications.
- Cross-Platform Compatibility: Designed to support PrivMX on multiple operating systems and
  environments.
- Simple API: Easy-to-use interface for C# developers without compromising security.

## Library

PrivMX Endpoint C# is the fundamental wrapper library, essential for the Platform’s operational
functionality.
As the most minimalist library available, it provides the highest degree of flexibility in
customizing the Platform to meet your specific requirements.

This library implements models, exception catching, and the following modules:

- `CryptoApi` - Cryptographic methods used to encrypt/decrypt and sign your data or generate keys to
  work with PrivMX.
- `Connection` - Methods for managing connection with PrivMX.
- `ThreadApi` - Methods for managing Threads and sending/reading messages.
- `StoreApi` - Methods for managing Stores and sending/reading files.
- `InboxApi` - Methods for managing Inboxes and entries.

## Usage

For more details about PrivMX and PrivMX Endpoint C#, including setup guides and API reference, visit [PrivMX documentation](https://docs.privmx.dev).

## License Information

**PrivMX Endpoint C#**\
Copyright © 2024 Simplito sp. z o.o.

This project is part of the PrivMX Platform (https://privmx.dev). \
This project is Licensed under the MIT License.

PrivMX Endpoint and PrivMX Bridge are licensed under the PrivMX Free License.\
See the License for the specific language governing permissions and limitations under the License.

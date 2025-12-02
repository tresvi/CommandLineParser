using Tresvi.CommandParser.Exceptions;
using NUnit.Framework;
using Test_CommandParser.Models;
using Tresvi.CommandParser;

namespace Test_CommandParser
{
    [TestFixture]
    public class IPValidationAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        #region IPv4 and IPv6 Tests

        [TestCase(@"--ipaddress 192.168.1.1")]
        [TestCase(@"--ipaddress 10.0.0.1")]
        [TestCase(@"--ipaddress 172.16.0.1")]
        [TestCase(@"--ipaddress 255.255.255.255")]
        [TestCase(@"--ipaddress 0.0.0.0")]
        [TestCase(@"--ipaddress 2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        [TestCase(@"--ipaddress 2001:db8:85a3::8a2e:370:7334")]
        [TestCase(@"--ipaddress ::1")]
        [TestCase(@"--ipaddress fe80::1")]
        public void Parse_IPValidation_IPv4_IPv6_ValidIPs_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_IPValidation_IPv4_IPv6 result = CommandLine.Parse<Params_With_IPValidation_IPv4_IPv6>(args);

            Assert.IsNotNull(result.IPAddress);
            Assert.AreEqual(args[1], result.IPAddress);
        }

        [TestCase(@"--ipaddress invalid.ip", "invalid.ip")]
        [TestCase(@"--ipaddress 256.256.256.256", "256.256.256.256")]
        [TestCase(@"--ipaddress 192.168.1.1.1", "192.168.1.1.1")]
        [TestCase(@"--ipaddress notanip", "notanip")]
        [TestCase(@"--ipaddress ", "")]
        public void Parse_IPValidation_IPv4_IPv6_InvalidIPs_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IPValidation_IPv4_IPv6>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue.Trim()).Or.Contain("no puede estar vacío"));
        }

        #endregion

        #region IPv4 Only Tests

        [TestCase(@"--ipaddress 192.168.1.1")]
        [TestCase(@"--ipaddress 10.0.0.1")]
        [TestCase(@"--ipaddress 127.0.0.1")]
        public void Parse_IPValidation_IPv4_Only_ValidIPv4_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_IPValidation_IPv4_Only result = CommandLine.Parse<Params_With_IPValidation_IPv4_Only>(args);

            Assert.IsNotNull(result.IPAddress);
            Assert.AreEqual(args[1], result.IPAddress);
        }

        [TestCase(@"--ipaddress 2001:0db8:85a3:0000:0000:8a2e:0370:7334", "2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        [TestCase(@"--ipaddress ::1", "::1")]
        public void Parse_IPValidation_IPv4_Only_IPv6_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IPValidation_IPv4_Only>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("IPv6").Or.Contain("IPv4"));
        }

        #endregion

        #region IPv6 Only Tests

        [TestCase(@"--ipaddress 2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        [TestCase(@"--ipaddress ::1")]
        [TestCase(@"--ipaddress fe80::1")]
        public void Parse_IPValidation_IPv6_Only_ValidIPv6_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_IPValidation_IPv6_Only result = CommandLine.Parse<Params_With_IPValidation_IPv6_Only>(args);

            Assert.IsNotNull(result.IPAddress);
            Assert.AreEqual(args[1], result.IPAddress);
        }

        [TestCase(@"--ipaddress 192.168.1.1", "192.168.1.1")]
        [TestCase(@"--ipaddress 10.0.0.1", "10.0.0.1")]
        public void Parse_IPValidation_IPv6_Only_IPv4_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IPValidation_IPv6_Only>(args));

            Assert.That(exception?.Message, Does.Contain(invalidValue));
            Assert.That(exception?.Message, Does.Contain("IPv4").Or.Contain("IPv6"));
        }

        #endregion

        #region IP and Email Validation Combined Tests

        [TestCase(@"--serverip 192.168.1.1 --adminemail admin@example.com")]
        [TestCase(@"--serverip 10.0.0.1 --adminemail user@test.com")]
        public void Parse_IP_And_Email_Validation_ValidValues_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_IP_And_Email_Validation result = CommandLine.Parse<Params_With_IP_And_Email_Validation>(args);

            Assert.IsNotNull(result.ServerIP);
            Assert.IsNotNull(result.AdminEmail);
        }

        [TestCase(@"--serverip invalid.ip --adminemail admin@example.com", "invalid.ip")]
        public void Parse_IP_And_Email_Validation_InvalidIP_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IP_And_Email_Validation>(args));
            Assert.That(exception?.Message, Does.Contain(invalidValue));
        }

        [TestCase(@"--serverip 192.168.1.1 --adminemail invalid.email", "invalid.email")]
        public void Parse_IP_And_Email_Validation_InvalidEmail_Throws(string inputLine, string invalidValue)
        {
            string[] args = inputLine.Split(' ');
            InvalidEmailAddressException? exception = Assert.Throws<InvalidEmailAddressException>(
                () => CommandLine.Parse<Params_With_IP_And_Email_Validation>(args));
            Assert.That(exception?.Message, Does.Contain(invalidValue));
        }

        #endregion

        #region IP with Port Tests

        [TestCase(@"--ipaddress 192.168.1.1:8080")]
        [TestCase(@"--ipaddress 10.0.0.1:443")]
        [TestCase(@"--ipaddress 127.0.0.1:3000")]
        [TestCase(@"--ipaddress 172.16.0.1:65535")]
        [TestCase(@"--ipaddress 192.168.1.1:1")]
        public void Parse_IPValidation_WithPort_IPv4_ValidIPs_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_IPValidation_WithPort result = CommandLine.Parse<Params_With_IPValidation_WithPort>(args);

            Assert.IsNotNull(result.IPAddress);
            Assert.AreEqual(args[1], result.IPAddress);
        }

        [TestCase(@"--ipaddress [::1]:8080")]
        [TestCase(@"--ipaddress [2001:0db8:85a3:0000:0000:8a2e:0370:7334]:443")]
        [TestCase(@"--ipaddress [2001:db8:85a3::8a2e:370:7334]:3000")]
        [TestCase(@"--ipaddress [fe80::1]:65535")]
        public void Parse_IPValidation_WithPort_IPv6_ValidIPs_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_IPValidation_WithPort result = CommandLine.Parse<Params_With_IPValidation_WithPort>(args);

            Assert.IsNotNull(result.IPAddress);
            Assert.AreEqual(args[1], result.IPAddress);
        }

        [TestCase(@"--ipaddress 192.168.1.1:0", "0")]
        [TestCase(@"--ipaddress 10.0.0.1:65536", "65536")]
        [TestCase(@"--ipaddress 127.0.0.1:99999", "99999")]
        [TestCase(@"--ipaddress 192.168.1.1:abc", "abc")]
        public void Parse_IPValidation_WithPort_InvalidPort_Throws(string inputLine, string invalidPort)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IPValidation_WithPort>(args));

            Assert.That(exception?.Message, Does.Contain(invalidPort).Or.Contain("puerto"));
        }

        [TestCase(@"--ipaddress 192.168.1.1")]
        [TestCase(@"--ipaddress 10.0.0.1")]
        public void Parse_IPValidation_WithPort_PortIsOptional_OK(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            Params_With_IPValidation_WithPort result = CommandLine.Parse<Params_With_IPValidation_WithPort>(args);

            Assert.IsNotNull(result.IPAddress);
            Assert.AreEqual(args[1], result.IPAddress);
        }

        [TestCase(@"--ipaddress 192.168.1.1")]
        [TestCase(@"--ipaddress [::1]")]
        public void Parse_IPValidation_PortRequired_IPWithoutPort_Throws(string inputLine)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IPValidation_PortRequired>(args));

            Assert.That(exception?.Message, Does.Contain("debe incluir un puerto"));
        }

        [TestCase(@"--ipaddress 192.168.1.1:8080", "puerto")]
        [TestCase(@"--ipaddress [::1]:8080", "puerto")]
        public void Parse_IPValidation_WithoutPort_IPWithPort_Throws(string inputLine, string expectedMessage)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IPValidation_IPv4_IPv6>(args));

            Assert.That(exception?.Message, Does.Contain(expectedMessage).Or.Contain("no permite puertos"));
        }

        [TestCase(@"--ipaddress [::1:8080", "formato")]
        [TestCase(@"--ipaddress ::1]:8080", "formato")]
        public void Parse_IPValidation_WithPort_IPv6_InvalidFormat_Throws(string inputLine, string expectedMessage)
        {
            string[] args = inputLine.Split(' ');
            InvalidIPAddressException? exception = Assert.Throws<InvalidIPAddressException>(
                () => CommandLine.Parse<Params_With_IPValidation_WithPort>(args));

            Assert.That(exception?.Message, Does.Contain("formato").Or.Contain("no es una dirección IP válida").Or.Contain("[IP]:puerto"));
        }

        #endregion
    }
}


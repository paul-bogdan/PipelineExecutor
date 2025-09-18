# Security Standards for PipelineExecutor

## Reporting a Security Vulnerability

If you discover a security vulnerability in this project, please follow these steps:

1. **Do not create a public GitHub issue.**  
   This is to prevent exposing the vulnerability to others before a fix is available.

2. **Contact the maintainer privately**:  
   Email: **[your-email@example.com]**  
   Please provide:
   - A detailed description of the vulnerability.
   - Steps to reproduce it.
   - Any potential impact or risk.

3. The maintainer will respond within **48 hours** and provide guidance on next steps.

---

## Security Best Practices

To help maintain a secure codebase:

- **Validate all inputs**: Always sanitize and validate user or external input.  
- **Handle sensitive data carefully**: Do not hardcode secrets, passwords, or API keys.  
- **Keep dependencies updated**: Regularly check for security updates in NuGet packages.  
- **Use secure coding practices**: Avoid unsafe operations or untrusted code execution.  
- **Error handling**: Do not expose stack traces or sensitive info in production environments.  

---

## Dependency Security

- Use `dotnet list package --vulnerable` to check for known vulnerabilities in dependencies.  
- Prefer stable and actively maintained packages.  

---

## Disclosure Policy

- We aim to respond to all valid security reports promptly.  
- After a fix is available, a public advisory or release note may be published.  

---

## Additional Resources

- [.NET Security Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/security/)  
- [OWASP Top Ten](https://owasp.org/www-project-top-ten/)

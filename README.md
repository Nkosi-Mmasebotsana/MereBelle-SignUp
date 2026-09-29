# Mère Belle — Sign Up Screen

**beauty, lovingly nurtured**

A WPF (C#, .NET 8) desktop application built for the "Sign Up" activity. New employees
can register their own account instead of an admin creating it manually. On successful
registration, the app sends a real confirmation email (via the Brevo API) with a 6-digit
code, and the account cannot be used until that code is verified.

## Features

- Full Name, Email, Password, and Confirm Password fields
- Client-side validation:
  - All fields required
  - Email must contain `@` and a `.`
  - Password and Confirm Password must match
  - Duplicate emails are blocked
- Real confirmation emails sent through the [Brevo](https://www.brevo.com) transactional
  email API (not a simulation)
- 6-digit verification code screen — the account stays locked (`IsConfirmed = false`)
  until the correct code is entered
- Passwords are never stored in plain text — hashed and salted using
  `Rfc2898DeriveBytes.Pbkdf2` (PBKDF2-SHA256, 100,000 iterations)
- Users are persisted locally in `users.json`
- Custom pink UI theme, branded for Mère Belle

## Tech stack

- C# / .NET 8
- WPF (Windows Presentation Foundation)
- Brevo Transactional Email API (`api.brevo.com/v3/smtp/email`)

## Setup instructions

This app requires two environment variables to send emails. It will not run without them.

1. Create a free account at [brevo.com](https://www.brevo.com).
2. Verify a sender email address under **Senders, Domains & IPs → Senders**.
3. Generate an API key under **SMTP & API → API Keys**.
4. Add the following **user environment variables** on your machine:

   | Variable name          | Value                              |
   |------------------------|------------------------------------|
   | `BREVO_API_KEY`        | Your Brevo API key                 |
   | `BREVO_SENDER_EMAIL`   | Your verified Brevo sender email   |

5. Restart Visual Studio (or your terminal) so it picks up the new variables.
6. Open `MereBelle.sln` in Visual Studio 2022+, press **F5** to run.

## How to test

1. Run the app and fill in the Sign Up form with a real email address you can access.
2. Click **Create Account**.
3. Check the error handling:
   - Try mismatched passwords
   - Try an invalid email (no `@` or `.`)
   - Try signing up with an email already used
4. On success, check your inbox for the Mère Belle confirmation email.
5. Enter the 6-digit code in the Verify screen to activate the account.

## Project structure

| File                        | Purpose                                          |
|-----------------------------|--------------------------------------------------|
| `SignUpWindow.xaml(.cs)`    | Main registration screen and validation logic    |
| `VerifyCodeWindow.xaml(.cs)`| 6-digit confirmation code screen                 |
| `User.cs`                   | User data model                                  |
| `UserStore.cs`              | Reads/writes `users.json`, checks duplicates     |
| `PasswordHasher.cs`         | PBKDF2 password hashing and verification         |
| `EmailService.cs`           | Sends confirmation emails via the Brevo API      |

## Notes

- `users.json` is excluded from version control (see `.gitignore`) since it stores
  registered account data.
- No API keys are hardcoded in this repository — they are read from environment
  variables at runtime.

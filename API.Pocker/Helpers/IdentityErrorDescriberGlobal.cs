﻿using Microsoft.AspNetCore.Identity;

namespace API.Pocker.Helpers
{
    public class IdentityErrorDescriberGlobal: IdentityErrorDescriber
    {
        public override IdentityError ConcurrencyFailure()
        {
            return base.ConcurrencyFailure();
        }
        public override IdentityError DefaultError()
        {
            return base.DefaultError();
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return base.DuplicateEmail(email);
        }
        public override IdentityError DuplicateRoleName(string role)
        {
            return base.DuplicateRoleName(role);
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return base.DuplicateUserName(userName);
        }
        public override IdentityError InvalidEmail(string email)
        {
            return base.InvalidEmail(email);
        }
        public override IdentityError InvalidRoleName(string role)
        {
            return base.InvalidRoleName(role);
        }
        public override IdentityError InvalidUserName(string userName)
        {
            return base.InvalidUserName(userName);
        }
        public override IdentityError LoginAlreadyAssociated()
        {
            return base.LoginAlreadyAssociated();
        }
        public override IdentityError PasswordMismatch()
        {
            return base.PasswordMismatch();
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return base.PasswordRequiresDigit();
        }
        public override IdentityError PasswordRequiresLower()
        {
            return base.PasswordRequiresLower();
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return base.PasswordRequiresNonAlphanumeric();
        }
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return base.PasswordRequiresUniqueChars(uniqueChars);
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return base.PasswordRequiresUpper();
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"The password must be a minimum lake of { length } characters."
            };
        }
        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return base.RecoveryCodeRedemptionFailed();
        }
        public override IdentityError UserAlreadyHasPassword()
        {
            return base.UserAlreadyHasPassword();
        }
        public override IdentityError UserAlreadyInRole(string role)
        {
            return base.UserAlreadyInRole(role);
        }
        public override IdentityError UserLockoutNotEnabled()
        {
            return base.UserLockoutNotEnabled();
        }
        public override IdentityError UserNotInRole(string role)
        {
            return base.UserNotInRole(role);
        }
        public override IdentityError InvalidToken()
        {
            return base.InvalidToken();
        }
    }
}

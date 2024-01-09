import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { min } from 'rxjs';

export function passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value: string = control.value || '';

        const minLength = 8;
        if (value.length < minLength) {
            return { minLength: true };
        }
        // Check for at least one uppercase letter
        const uppercaseRegex = /[A-Z]/;
        if (!uppercaseRegex.test(value)) {
            return { uppercase: true };
        }

        // Check for at least one number
        const numberRegex = /\d/;
        if (!numberRegex.test(value)) {
            return { number: true };
        }

        // Check for at least one special character
        const specialCharacterRegex = /[!@#$%^&*(),.?":{}|<>]/;
        if (!specialCharacterRegex.test(value)) {
            return { specialCharacter: true };
        }

        // Password is valid
        return null;
    };
}

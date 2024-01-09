import { AbstractControl, ValidatorFn } from '@angular/forms';

export function startWithUppercaseValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const value = control.value as string;

        if (value && value.length > 0) {
            const firstLetter = value[0];
            if (firstLetter !== firstLetter.toUpperCase()) {
                return { startWithUppercase: true };
            }
        }

        return null;
    };
}

import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const endNotBeforeStart: ValidatorFn = (form: AbstractControl): ValidationErrors | null => {
  const start = form.get('applicationStartDate')?.value;
  const end = form.get('applicationEndDate')?.value;

  return start && end && end < start ? { endBeforeStart: true } : null;
};

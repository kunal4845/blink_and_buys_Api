import { Component, Input } from '@angular/core';
import { AbstractControlDirective, AbstractControl } from '@angular/forms';

@Component({
    selector: 'show-errors',
    template: `
    <span *ngIf="shouldShowErrors()">
      <span
        class="text-danger"
        *ngFor="let error of listOfErrors()"
        >{{ error }}</span
      >
    </span>
  `
})
export class ShowErrorsComponent {
    @Input() public control: AbstractControlDirective | AbstractControl;
    @Input('name') name: string;

    constructor() { }

    private static readonly errorMessages = {
        required: (params, fileName) => fileName + ' is required',
        minlength: params =>
            'The min number of characters is ' + params.requiredLength,
        maxlength: params =>
            'The max allowed number of characters is ' + params.requiredLength,
        pattern: (params, fileName) => fileName +' is not valid',
        years: params => params.message,
        countryCity: params => params.message,
        uniqueName: params => params.message,
        telephoneNumbers: params => params.message,
        telephoneNumber: params => params.message
    };

    shouldShowErrors(): boolean {
        return (
            this.control &&
            this.control.errors &&
            (this.control.dirty || this.control.touched)
        );
    }

    listOfErrors(): string[] {
        return Object.keys(this.control.errors).map(field =>
            this.getMessage(field, this.control.errors[field])
        );
    }

    private getMessage(type: string, params: any) {
        return ShowErrorsComponent.errorMessages[type](params, this.name);
    }
}
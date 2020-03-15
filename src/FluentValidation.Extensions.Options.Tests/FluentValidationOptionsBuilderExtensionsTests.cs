using System;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;

namespace FluentValidation.Extensions.Options.Tests
{
    public class FluentValidationOptionsBuilderExtensionsTests
    {
        [Fact]
        public void Validate_with_validator_type_passing_valid_options_will_pass()
        {
            Validate_passing_valid_options_will_pass(
                x => x.Validate<MyOptions, MyOptionsValidator>()
                );
        }

        [Fact]
        public void Validate_with_validator_type_passing_invalid_options_will_throw_exception()
        {
            Validate_passing_invalid_options_will_throw_exception(
                x => x.Validate<MyOptions, MyOptionsValidator>()
                );
        }

        [Fact]
        public void Validate_with_validator_instance_passing_valid_options_will_pass()
        {
            Validate_passing_valid_options_will_pass(
                x => x.Validate(new MyOptionsValidator())
                );
        }

        [Fact]
        public void Validate_with_validator_instance_passing_invalid_options_will_throw_exception()
        {
            Validate_passing_invalid_options_will_throw_exception(
                x => x.Validate(new MyOptionsValidator())
                );
        }

        [Fact]
        public void Validate_with_validator_creator_passing_valid_options_will_pass()
        {
            Validate_passing_valid_options_will_pass(
                x => x.Validate(serviceProvider => new MyOptionsValidator())
                );
        }

        [Fact]
        public void Validate_with_validator_creator_passing_invalid_options_will_throw_exception()
        {
            Validate_passing_invalid_options_will_throw_exception(
                x => x.Validate(serviceProvider => new MyOptionsValidator())
                );
        }

        [Fact]
        public void FluentValidate_with_validator_type_passing_valid_options_will_pass()
        {
            Validate_passing_valid_options_will_pass(
                x => x.FluentValidate().With<MyOptionsValidator>()
                );
        }

        [Fact]
        public void FluentValidate_with_validator_type_passing_invalid_options_will_throw_exception()
        {
            Validate_passing_invalid_options_will_throw_exception(
                x => x.FluentValidate().With<MyOptionsValidator>()
                );
        }

        [Fact]
        public void FluentValidate_with_validator_instance_passing_valid_options_will_pass()
        {
            Validate_passing_valid_options_will_pass(
                x => x.FluentValidate().With(new MyOptionsValidator())
                );
        }

        [Fact]
        public void FluentValidate_with_validator_instance_passing_invalid_options_will_throw_exception()
        {
            Validate_passing_invalid_options_will_throw_exception(
                x => x.FluentValidate().With(new MyOptionsValidator())
                );
        }

        [Fact]
        public void FluentValidate_with_validator_creator_passing_valid_options_will_pass()
        {
            Validate_passing_valid_options_will_pass(
                x => x.FluentValidate().With(serviceProvider => new MyOptionsValidator())
                );
        }

        [Fact]
        public void FluentValidate_with_validator_creator_passing_invalid_options_will_throw_exception()
        {
            Validate_passing_invalid_options_will_throw_exception(
                x => x.FluentValidate().With(serviceProvider => new MyOptionsValidator())
                );
        }

        private void Validate_passing_valid_options_will_pass(
            Func<OptionsBuilder<MyOptions>, OptionsBuilder<MyOptions>> addValidation
            )
        {
            var services = new ServiceCollection();
            var optionsBuilder = services.AddOptions<MyOptions>()
                .Configure(options => options.TrueValue = true);
            addValidation(optionsBuilder);
            using var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<MyOptions>>();
            options.Value.TrueValue.Should().BeTrue();
        }

        private void Validate_passing_invalid_options_will_throw_exception(
            Func<OptionsBuilder<MyOptions>, OptionsBuilder<MyOptions>> addValidation
            )
        {
            var services = new ServiceCollection();
            var optionsBuilder = services.AddOptions<MyOptions>()
                .Configure(options => options.TrueValue = false);
            addValidation(optionsBuilder);
            using var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<MyOptions>>();
            options.Invoking(x => x.Value)
                .Should().Throw<OptionsValidationException>()
                .And
                .OptionsType.Should().Be(typeof(MyOptions));
        }
    }
}

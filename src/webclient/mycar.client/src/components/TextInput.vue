<template>
  <label :for="props.id">{{props.label}}</label>
  <input :type="props.type"
    :id="props.id"
    :value="props.modelValue"
    :placeholder="props.placeholder"
    @input="$emit('input',($event.target as HTMLInputElement).value)"/>
  <div class='hint-error' v-if="props.showHint">{{props.hint}}</div>
</template>

<script setup lang='ts'>

  interface Props {
    modelValue: string | boolean,
    id: string,
    label?: string,
    placeholder?: string,
    hint?: string,
    showHint?: boolean,
    readonly?: boolean,
    type: 'text' | 'password'
  }

  const props = withDefaults(defineProps<Props>(), {
    modelValue: '',
    hint: '',
    label: '',
    placeholder: '',
    readonly: false,
    showHint: false,
    type: 'text'
  });

  const emit = defineEmits(['input']);
</script>

<style scoped>
    input {
        width: 100%;
        padding: 8px;
        border: 1px solid #ccc;
        border-radius: 5px;
    }
</style>
